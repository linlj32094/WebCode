// Monaco Editor 辅助脚本
let monacoEditor = null;

// 初始化 Monaco Editor
export function initMonacoEditor() {
    // 检查 Monaco 是否已加载
    if (typeof monaco === 'undefined') {
        console.warn('Monaco Editor 未加载，正在动态加载...');
        loadMonacoEditor();
        return;
    }

    const container = document.getElementById('monaco-editor');
    if (!container) {
        console.error('Monaco Editor 容器未找到');
        return;
    }

    // 如果已经存在编辑器实例，先销毁
    if (monacoEditor) {
        monacoEditor.dispose();
    }

    // 创建 Monaco Editor
    monacoEditor = monaco.editor.create(container, {
        value: '// 代码将在这里显示...\n',
        language: 'javascript',
        theme: 'vs-dark',
        automaticLayout: true,
        readOnly: false,
        minimap: {
            enabled: true
        },
        scrollBeyondLastLine: false,
        fontSize: 14,
        lineNumbers: 'on',
        renderWhitespace: 'selection',
        wordWrap: 'on'
    });

    console.log('Monaco Editor 初始化成功');
}

// 更新 Monaco Editor 内容
export function updateMonacoContent(content) {
    if (!monacoEditor) {
        console.warn('Monaco Editor 未初始化');
        return;
    }

    // 检测代码语言
    const language = detectLanguage(content);
    
    // 更新模型语言
    const model = monacoEditor.getModel();
    if (model) {
        monaco.editor.setModelLanguage(model, language);
    }

    // 更新内容
    monacoEditor.setValue(content || '// 代码将在这里显示...\n');
}

// 检测代码语言
function detectLanguage(content) {
    if (!content) return 'plaintext';

    // 简单的语言检测
    if (content.includes('```')) {
        const match = content.match(/```(\w+)/);
        if (match && match[1]) {
            const lang = match[1].toLowerCase();
            const supportedLanguages = ['javascript', 'typescript', 'python', 'java', 'csharp', 'cpp', 'c', 'go', 'rust', 'html', 'css', 'json', 'xml', 'sql', 'bash', 'powershell'];
            if (supportedLanguages.includes(lang)) {
                return lang === 'csharp' ? 'csharp' : lang;
            }
        }
    }

    // 根据内容特征检测
    if (content.includes('function') || content.includes('const') || content.includes('let') || content.includes('=>')) {
        return 'javascript';
    }
    if (content.includes('def ') || content.includes('import ') || content.includes('print(')) {
        return 'python';
    }
    if (content.includes('public class') || content.includes('namespace') || content.includes('using System')) {
        return 'csharp';
    }
    if (content.includes('<html') || content.includes('<!DOCTYPE')) {
        return 'html';
    }
    if (content.includes('{') && content.includes('}') && content.includes(':')) {
        return 'json';
    }

    return 'plaintext';
}

// 动态加载 Monaco Editor
function loadMonacoEditor() {
    const loaderScript = document.createElement('script');
    loaderScript.src = 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs/loader.min.js';
    loaderScript.onload = () => {
        require.config({ 
            paths: { 
                'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs' 
            } 
        });
        require(['vs/editor/editor.main'], () => {
            console.log('Monaco Editor 加载完成');
            initMonacoEditor();
        });
    };
    loaderScript.onerror = () => {
        console.error('Monaco Editor 加载失败');
    };
    document.head.appendChild(loaderScript);
}

// 获取编辑器内容
export function getMonacoContent() {
    if (!monacoEditor) {
        return '';
    }
    return monacoEditor.getValue();
}

// 销毁编辑器
export function disposeMonacoEditor() {
    if (monacoEditor) {
        monacoEditor.dispose();
        monacoEditor = null;
    }
}

