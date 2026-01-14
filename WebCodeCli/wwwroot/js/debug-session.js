/**
 * 会话调试工具
 * 在浏览器控制台中使用这些函数来调试会话存储
 */

window.debugSession = {
    /**
     * 查看所有会话
     */
    viewAll: function() {
        const sessions = window.webCliSessionStorage.loadSessions();
        console.group('📋 所有会话');
        console.log(`共 ${sessions.length} 个会话`);
        sessions.forEach((session, index) => {
            console.log(`\n${index + 1}. ${session.title}`);
            console.log(`   ID: ${session.sessionId}`);
            console.log(`   消息数: ${session.messages?.length || 0}`);
            console.log(`   创建时间: ${session.createdAt}`);
            console.log(`   更新时间: ${session.updatedAt}`);
            console.log(`   工作区: ${session.workspacePath}`);
        });
        console.groupEnd();
        return sessions;
    },
    
    /**
     * 查看单个会话详情
     */
    viewSession: function(sessionId) {
        const session = window.webCliSessionStorage.getSession(sessionId);
        if (!session) {
            console.error(`会话不存在: ${sessionId}`);
            return null;
        }
        
        console.group(`📄 会话详情: ${session.title}`);
        console.log('ID:', session.sessionId);
        console.log('标题:', session.title);
        console.log('消息数:', session.messages?.length || 0);
        console.log('创建时间:', session.createdAt);
        console.log('更新时间:', session.updatedAt);
        console.log('工作区:', session.workspacePath);
        console.log('工具ID:', session.toolId);
        
        if (session.messages && session.messages.length > 0) {
            console.log('\n消息列表:');
            session.messages.forEach((msg, index) => {
                console.log(`  ${index + 1}. [${msg.role}] ${msg.content.substring(0, 50)}...`);
            });
        }
        console.groupEnd();
        
        return session;
    },
    
    /**
     * 查看 localStorage 原始数据
     */
    viewRaw: function() {
        const raw = localStorage.getItem('webcli_sessions');
        console.group('🔍 localStorage 原始数据');
        console.log('键名: webcli_sessions');
        console.log('数据长度:', raw ? raw.length : 0);
        console.log('数据大小:', raw ? `${(raw.length / 1024).toFixed(2)} KB` : '0 KB');
        
        if (raw) {
            try {
                const parsed = JSON.parse(raw);
                console.log('解析成功，会话数:', Array.isArray(parsed) ? parsed.length : 'N/A');
                console.log('数据:', parsed);
            } catch (e) {
                console.error('解析失败:', e.message);
                console.log('原始数据:', raw.substring(0, 500) + '...');
            }
        } else {
            console.log('无数据');
        }
        console.groupEnd();
    },
    
    /**
     * 测试保存功能
     */
    testSave: function() {
        const testSession = {
            sessionId: 'test-' + Date.now(),
            title: '测试会话 ' + new Date().toLocaleTimeString(),
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString(),
            workspacePath: 'C:\\test',
            toolId: 'test-tool',
            messages: [
                {
                    role: 'user',
                    content: '测试消息',
                    isCompleted: true
                },
                {
                    role: 'assistant',
                    content: '测试回复',
                    isCompleted: true
                }
            ],
            isWorkspaceValid: true
        };
        
        console.log('🧪 测试保存会话:', testSession.title);
        
        try {
            const sessions = window.webCliSessionStorage.loadSessions();
            sessions.push(testSession);
            const success = window.webCliSessionStorage.saveSessions(sessions);
            
            if (success) {
                console.log('✅ 保存成功');
                console.log('当前会话总数:', sessions.length);
                return testSession;
            } else {
                console.error('❌ 保存失败');
                return null;
            }
        } catch (e) {
            console.error('❌ 保存异常:', e.message);
            return null;
        }
    },
    
    /**
     * 清空所有会话
     */
    clearAll: function() {
        if (confirm('确定要清空所有会话吗？此操作不可恢复！')) {
            window.webCliSessionStorage.clearSessions();
            console.log('✅ 已清空所有会话');
        }
    },
    
    /**
     * 查看存储使用情况
     */
    viewStorage: function() {
        const info = window.webCliSessionStorage.getStorageInfo();
        console.group('💾 存储使用情况');
        console.log(`已使用: ${info.usedMB} MB`);
        console.log(`总容量: ${info.limitMB} MB`);
        console.log(`使用率: ${info.percentage}%`);
        console.groupEnd();
        return info;
    },
    
    /**
     * 显示帮助信息
     */
    help: function() {
        console.log(`
🔧 会话调试工具使用说明

可用命令：
  debugSession.viewAll()           - 查看所有会话
  debugSession.viewSession(id)     - 查看单个会话详情
  debugSession.viewRaw()           - 查看 localStorage 原始数据
  debugSession.testSave()          - 测试保存功能
  debugSession.clearAll()          - 清空所有会话
  debugSession.viewStorage()       - 查看存储使用情况
  debugSession.help()              - 显示此帮助信息

示例：
  debugSession.viewAll()
  debugSession.viewSession('your-session-id')
  debugSession.testSave()
        `);
    }
};

// 自动显示帮助信息
console.log('🔧 会话调试工具已加载，输入 debugSession.help() 查看使用说明');
