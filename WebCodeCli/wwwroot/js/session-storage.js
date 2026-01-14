// 立即执行日志，用于调试
console.log('🔧 session-storage.js 正在加载...');

/**
 * 会话存储管理 - localStorage 交互层
 */
window.webCliSessionStorage = {
    /**
     * 保存所有会话到 localStorage
     * @param {Array} sessions - 会话数组
     * @returns {boolean} - 是否保存成功
     */
    saveSessions: function(sessions) {
        try {
            if (!sessions || !Array.isArray(sessions)) {
                console.error('保存会话失败: 无效的会话数据');
                return false;
            }

            const json = JSON.stringify(sessions);
            localStorage.setItem('webcli_sessions', json);
            
            console.log(`成功保存 ${sessions.length} 个会话到 localStorage`);
            return true;
        } catch (e) {
            if (e.name === 'QuotaExceededError') {
                console.error('保存会话失败: localStorage 空间不足', e);
                // 抛出特定错误以便 C# 代码捕获
                throw new Error('QuotaExceededError: localStorage 空间不足');
            } else {
                console.error('保存会话失败:', e);
                return false;
            }
        }
    },
    
    /**
     * 从 localStorage 加载所有会话
     * @returns {Array} - 会话数组
     */
    loadSessions: function() {
        try {
            const json = localStorage.getItem('webcli_sessions');
            
            if (!json) {
                console.log('未找到会话数据，返回空数组');
                return [];
            }

            const sessions = JSON.parse(json);
            
            if (!Array.isArray(sessions)) {
                console.warn('会话数据格式不正确，返回空数组');
                return [];
            }

            console.log(`成功加载 ${sessions.length} 个会话`);
            return sessions;
        } catch (e) {
            console.error('加载会话失败:', e);
            
            // 如果 JSON 解析失败，清空损坏的数据
            if (e instanceof SyntaxError) {
                console.warn('会话数据已损坏，正在清空...');
                try {
                    localStorage.removeItem('webcli_sessions');
                } catch (clearError) {
                    console.error('清空损坏数据失败:', clearError);
                }
            }
            
            return [];
        }
    },
    
    /**
     * 获取 localStorage 使用情况
     * @returns {Object} - 包含 used 和 limit 的对象
     */
    getStorageInfo: function() {
        try {
            let total = 0;
            
            // 计算所有 localStorage 项的大小
            for (let key in localStorage) {
                if (localStorage.hasOwnProperty(key)) {
                    const value = localStorage[key];
                    // 每个字符在 UTF-16 中占用 2 字节
                    total += (key.length + value.length) * 2;
                }
            }
            
            // 大多数浏览器限制为 5-10MB，这里使用 5MB 作为保守估计
            const limit = 5 * 1024 * 1024;
            
            return {
                used: total,
                limit: limit,
                usedMB: (total / (1024 * 1024)).toFixed(2),
                limitMB: (limit / (1024 * 1024)).toFixed(2),
                percentage: ((total / limit) * 100).toFixed(2)
            };
        } catch (e) {
            console.error('获取存储信息失败:', e);
            return {
                used: 0,
                limit: 5 * 1024 * 1024,
                usedMB: '0.00',
                limitMB: '5.00',
                percentage: '0.00'
            };
        }
    },
    
    /**
     * 清空所有会话数据
     * @returns {boolean} - 是否清空成功
     */
    clearSessions: function() {
        try {
            localStorage.removeItem('webcli_sessions');
            console.log('已清空所有会话数据');
            return true;
        } catch (e) {
            console.error('清空会话数据失败:', e);
            return false;
        }
    },
    
    /**
     * 获取单个会话
     * @param {string} sessionId - 会话ID
     * @returns {Object|null} - 会话对象或 null
     */
    getSession: function(sessionId) {
        try {
            const sessions = this.loadSessions();
            const session = sessions.find(s => s.sessionId === sessionId);
            return session || null;
        } catch (e) {
            console.error('获取会话失败:', e);
            return null;
        }
    },
    
    /**
     * 删除单个会话
     * @param {string} sessionId - 会话ID
     * @returns {boolean} - 是否删除成功
     */
    deleteSession: function(sessionId) {
        try {
            const sessions = this.loadSessions();
            const filteredSessions = sessions.filter(s => s.sessionId !== sessionId);
            
            if (filteredSessions.length === sessions.length) {
                console.warn(`会话 ${sessionId} 不存在`);
                return false;
            }
            
            return this.saveSessions(filteredSessions);
        } catch (e) {
            console.error('删除会话失败:', e);
            return false;
        }
    }
};

// 在页面加载时显示存储信息（仅用于调试）
if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
    window.addEventListener('load', function() {
        const info = window.webCliSessionStorage.getStorageInfo();
        console.log('localStorage 使用情况:', 
            `${info.usedMB}MB / ${info.limitMB}MB (${info.percentage}%)`);
    });
}


/**
 * 性能监控工具 - 使用 IndexedDB 存储
 */
window.sessionPerformance = {
    /**
     * 记录性能指标
     * @param {string} operation - 操作名称
     * @param {number} startTime - 开始时间（毫秒）
     * @param {number} endTime - 结束时间（毫秒）
     */
    logPerformance: function(operation, startTime, endTime) {
        const duration = endTime - startTime;
        const timestamp = new Date().toISOString();
        
        // 只在开发环境记录
        if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
            console.log(`[性能] ${operation}: ${duration.toFixed(2)}ms (${timestamp})`);
            
            // 如果操作超过 100ms，发出警告
            if (duration > 100) {
                console.warn(`[性能警告] ${operation} 耗时过长: ${duration.toFixed(2)}ms`);
            }
        }
        
        return duration;
    },
    
    /**
     * 测量函数执行时间
     * @param {Function} fn - 要测量的函数
     * @param {string} operationName - 操作名称
     * @returns {Promise<any>} - 函数执行结果
     */
    measureAsync: async function(fn, operationName) {
        const startTime = performance.now();
        try {
            const result = await fn();
            const endTime = performance.now();
            this.logPerformance(operationName, startTime, endTime);
            return result;
        } catch (error) {
            const endTime = performance.now();
            this.logPerformance(`${operationName} (失败)`, startTime, endTime);
            throw error;
        }
    },
    
    /**
     * 获取内存使用情况（如果浏览器支持）
     * @returns {Object|null} - 内存使用信息
     */
    getMemoryUsage: function() {
        if (performance.memory) {
            return {
                usedJSHeapSize: (performance.memory.usedJSHeapSize / 1024 / 1024).toFixed(2) + ' MB',
                totalJSHeapSize: (performance.memory.totalJSHeapSize / 1024 / 1024).toFixed(2) + ' MB',
                jsHeapSizeLimit: (performance.memory.jsHeapSizeLimit / 1024 / 1024).toFixed(2) + ' MB'
            };
        }
        return null;
    },
    
    /**
     * 记录会话操作性能 - 使用 IndexedDB
     * @param {string} operation - 操作类型（load, save, delete等）
     * @param {number} sessionCount - 会话数量
     * @param {number} duration - 耗时（毫秒）
     */
    recordSessionOperation: async function(operation, sessionCount, duration) {
        try {
            // 使用 IndexedDB 存储
            if (window.webCliIndexedDB && window.webCliIndexedDB.isReady()) {
                await window.webCliIndexedDB.recordPerformanceMetric(operation, sessionCount, duration);
            }
        } catch (e) {
            console.error('记录性能指标失败:', e);
        }
    },
    
    /**
     * 获取性能统计 - 使用 IndexedDB
     * @returns {Promise<Object|null>} - 性能统计信息
     */
    getPerformanceStats: async function() {
        try {
            // 使用 IndexedDB 获取
            if (window.webCliIndexedDB && window.webCliIndexedDB.isReady()) {
                return await window.webCliIndexedDB.getPerformanceStats();
            }
            return null;
        } catch (e) {
            console.error('读取性能指标失败:', e);
            return null;
        }
    },
    
    /**
     * 清除性能指标 - 使用 IndexedDB
     */
    clearPerformanceMetrics: async function() {
        try {
            if (window.webCliIndexedDB && window.webCliIndexedDB.isReady()) {
                await window.webCliIndexedDB.clearPerformanceMetrics();
                console.log('性能指标已清除');
            }
        } catch (e) {
            console.error('清除性能指标失败:', e);
        }
    }
};

// 在开发环境下，定期输出性能统计
if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
    // 每 5 分钟输出一次性能统计
    setInterval(async function() {
        const stats = await window.sessionPerformance.getPerformanceStats();
        if (stats) {
            console.group('📊 会话操作性能统计');
            Object.keys(stats).forEach(operation => {
                const stat = stats[operation];
                console.log(`${operation}:`, {
                    '操作次数': stat.count,
                    '平均耗时': `${stat.avgDuration.toFixed(2)}ms`,
                    '最小耗时': `${stat.minDuration.toFixed(2)}ms`,
                    '最大耗时': `${stat.maxDuration.toFixed(2)}ms`
                });
            });
            console.groupEnd();
            
            // 输出内存使用情况
            const memory = window.sessionPerformance.getMemoryUsage();
            if (memory) {
                console.log('💾 内存使用:', memory);
            }
        }
    }, 5 * 60 * 1000);
}


// 加载完成日志
console.log('✅ session-storage.js 加载完成');
console.log('✅ webCliSessionStorage 对象:', typeof window.webCliSessionStorage);
console.log('✅ sessionPerformance 对象:', typeof window.sessionPerformance);
