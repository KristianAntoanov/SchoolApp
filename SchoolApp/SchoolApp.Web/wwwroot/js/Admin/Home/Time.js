

function updateTime() {
    const now = new Date();
    document.getElementById('currentTime').textContent =
        now.toLocaleString('bg-BG', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
}
setInterval(updateTime, 1000);
updateTime();