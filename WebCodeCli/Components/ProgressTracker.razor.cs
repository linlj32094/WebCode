using Microsoft.AspNetCore.Components;

namespace WebCodeCli.Components;

public partial class ProgressTracker : ComponentBase, IDisposable
{
    [Parameter] public bool CanCancel { get; set; } = true;
    [Parameter] public EventCallback OnCancel { get; set; }

    private List<ProgressStage> _stages = new();
    private bool _isVisible = false;
    private double _progress = 0;
    private string _currentStageText = "";
    private string _elapsedTime = "0.0s";
    private string _detailMessage = "";
    private bool _canCancel = true;
    private bool _isCompleted = false;
    private bool _isFailed = false;
    
    private DateTime _startTime;
    private System.Threading.Timer? _timer;

    protected override void OnInitialized()
    {
        InitializeStages();
    }

    private void InitializeStages()
    {
        _stages = new List<ProgressStage>
        {
            new ProgressStage
            {
                Id = "thread.started",
                Name = "初始化",
                Icon = "🚀",
                Status = StageStatus.Pending,
                ProgressWeight = 10
            },
            new ProgressStage
            {
                Id = "turn.started",
                Name = "分析中",
                Icon = "💭",
                Status = StageStatus.Pending,
                ProgressWeight = 20
            },
            new ProgressStage
            {
                Id = "item.started",
                Name = "执行中",
                Icon = "⚙️",
                Status = StageStatus.Pending,
                ProgressWeight = 60
            },
            new ProgressStage
            {
                Id = "turn.completed",
                Name = "完成",
                Icon = "✅",
                Status = StageStatus.Pending,
                ProgressWeight = 10
            }
        };
    }

    public void Start()
    {
        _isVisible = true;
        _isCompleted = false;
        _isFailed = false;
        _startTime = DateTime.Now;
        _progress = 0;
        _canCancel = CanCancel;

        // 重置所有阶段状态
        foreach (var stage in _stages)
        {
            stage.Status = StageStatus.Pending;
        }

        // 启动第一个阶段
        UpdateStage("thread.started", StageStatus.Active);

        // 启动计时器
        _timer = new System.Threading.Timer(_ =>
        {
            InvokeAsync(() =>
            {
                UpdateElapsedTime();
                StateHasChanged();
            });
        }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

        StateHasChanged();
    }

    public void UpdateStage(string stageId, StageStatus status, string? detailMessage = null)
    {
        var stage = _stages.FirstOrDefault(s => s.Id == stageId);
        if (stage == null) return;

        // 完成之前的阶段
        var previousStages = _stages.TakeWhile(s => s.Id != stageId);
        foreach (var prev in previousStages)
        {
            if (prev.Status != StageStatus.Failed)
            {
                prev.Status = StageStatus.Completed;
            }
        }

        // 更新当前阶段
        stage.Status = status;
        _currentStageText = stage.Name;

        if (!string.IsNullOrEmpty(detailMessage))
        {
            _detailMessage = detailMessage;
        }

        // 计算进度
        CalculateProgress();

        // 检查是否完成
        if (stageId == "turn.completed" && status == StageStatus.Completed)
        {
            Complete();
        }

        StateHasChanged();
    }

    public void UpdateProgress(int itemCount, int completedItems)
    {
        var executionStage = _stages.FirstOrDefault(s => s.Id == "item.started");
        if (executionStage != null && executionStage.Status == StageStatus.Active)
        {
            // 根据完成的项目数量动态计算进度
            var baseProgress = _stages.TakeWhile(s => s.Id != "item.started")
                .Sum(s => s.Status == StageStatus.Completed ? s.ProgressWeight : 0);

            var executionProgress = itemCount > 0 
                ? (double)completedItems / itemCount * executionStage.ProgressWeight
                : 0;

            _progress = baseProgress + executionProgress;
            StateHasChanged();
        }
    }

    public void Fail(string errorMessage)
    {
        _isFailed = true;
        _canCancel = false;

        var activeStage = _stages.FirstOrDefault(s => s.Status == StageStatus.Active);
        if (activeStage != null)
        {
            activeStage.Status = StageStatus.Failed;
        }

        _currentStageText = "失败";
        _detailMessage = errorMessage;
        
        StopTimer();
        StateHasChanged();
    }

    public void Complete()
    {
        _isCompleted = true;
        _canCancel = false;
        _progress = 100;

        foreach (var stage in _stages)
        {
            stage.Status = StageStatus.Completed;
        }

        _currentStageText = "已完成";
        
        StopTimer();
        StateHasChanged();

        // 3秒后自动隐藏
        Task.Delay(3000).ContinueWith(_ =>
        {
            InvokeAsync(() =>
            {
                _isVisible = false;
                StateHasChanged();
            });
        });
    }

    public void Hide()
    {
        _isVisible = false;
        StopTimer();
        StateHasChanged();
    }

    private void CalculateProgress()
    {
        _progress = _stages
            .Where(s => s.Status == StageStatus.Completed)
            .Sum(s => s.ProgressWeight);

        // 活动阶段算50%
        var activeStage = _stages.FirstOrDefault(s => s.Status == StageStatus.Active);
        if (activeStage != null)
        {
            _progress += activeStage.ProgressWeight * 0.5;
        }

        _progress = Math.Min(_progress, 100);
    }

    private void UpdateElapsedTime()
    {
        var elapsed = DateTime.Now - _startTime;
        _elapsedTime = $"{elapsed.TotalSeconds:F1}s";
    }

    private void StopTimer()
    {
        _timer?.Dispose();
        _timer = null;
    }

    private async Task OnCancelClick()
    {
        if (OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync();
        }
        Hide();
    }

    private string GetStageClass(ProgressStage stage)
    {
        return stage.Status switch
        {
            StageStatus.Completed => "bg-gradient-to-br from-green-500 to-green-600",
            StageStatus.Active => "bg-gradient-to-br from-blue-500 to-blue-600 animate-pulse",
            StageStatus.Failed => "bg-gradient-to-br from-red-500 to-red-600",
            _ => "bg-gray-200"
        };
    }

    private string GetStageNameClass(ProgressStage stage)
    {
        return stage.Status switch
        {
            StageStatus.Completed => "text-green-600",
            StageStatus.Active => "text-blue-600",
            StageStatus.Failed => "text-red-600",
            _ => "text-gray-500"
        };
    }

    private string GetConnectorClass(ProgressStage stage)
    {
        return stage.Status == StageStatus.Completed 
            ? "bg-gradient-to-r from-green-500 to-green-600" 
            : "bg-gray-200";
    }

    private string GetProgressBarClass()
    {
        if (_isFailed)
        {
            return "bg-gradient-to-r from-red-500 to-red-600";
        }
        
        if (_isCompleted)
        {
            return "bg-gradient-to-r from-green-500 to-green-600";
        }

        return "bg-gradient-to-r from-blue-500 via-blue-600 to-indigo-600";
    }

    private string GetCurrentStageTextClass()
    {
        if (_isFailed)
        {
            return "text-red-600";
        }
        
        if (_isCompleted)
        {
            return "text-green-600";
        }

        return "text-blue-600";
    }

    public void Dispose()
    {
        StopTimer();
    }

    private class ProgressStage
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public StageStatus Status { get; set; }
        public double ProgressWeight { get; set; }
    }

    public enum StageStatus
    {
        Pending,
        Active,
        Completed,
        Failed
    }
}

