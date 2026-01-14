// 调试检查脚本
console.log('=== JavaScript 环境检查 ===');
console.log('1. window 对象:', typeof window);
console.log('2. localStorage:', typeof localStorage);
console.log('3. sessionStorage (原生):', typeof sessionStorage);
console.log('4. webCliSessionStorage:', typeof window.webCliSessionStorage);
console.log('5. sessionPerformance:', typeof window.sessionPerformance);

if (typeof window.webCliSessionStorage !== 'undefined') {
    console.log('✅ webCliSessionStorage 已加载');
    console.log('   - saveSessions:', typeof window.webCliSessionStorage.saveSessions);
    console.log('   - loadSessions:', typeof window.webCliSessionStorage.loadSessions);
    console.log('   - getStorageInfo:', typeof window.webCliSessionStorage.getStorageInfo);
} else {
    console.error('❌ webCliSessionStorage 未加载！');
}

if (typeof window.sessionPerformance !== 'undefined') {
    console.log('✅ sessionPerformance 已加载');
} else {
    console.error('❌ sessionPerformance 未加载！');
}

console.log('=== 检查完成 ===');
