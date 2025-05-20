document.addEventListener('DOMContentLoaded', function() {
    const themeToggle = document.getElementById('themeToggle');
  
    themeToggle.addEventListener('click', function() {
      const body = document.body;
      if (body.classList.contains('light-theme')) {
        body.classList.remove('light-theme');
        body.classList.add('dark-theme');
        localStorage.setItem('theme', 'dark'); 
      } else if (body.classList.contains('dark-theme')) {
        body.classList.remove('dark-theme');
        body.classList.add('light-theme');
         localStorage.setItem('theme', 'light'); 
      }
      else { 
         body.classList.add('dark-theme');
         localStorage.setItem('theme', 'dark'); 
      }
    });
  
    const savedTheme = localStorage.getItem('theme');
      if (savedTheme === 'dark') {
          document.body.classList.add('dark-theme');
      } else if (savedTheme === 'light') {
        document.body.classList.add('light-theme');
      } else {
        document.body.classList.add('light-theme'); 
      }
  });
  
    document.querySelector('.location-button').addEventListener('click', function() {
      const iframe = document.querySelector('.map-container iframe');
      const newSrc = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d33193.6505958905!2d60.56900037621643!3d56.83571661688658!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x43c16e8ea8b8589f%3A0x4bdc777e135a8eb1!2z0KbQtdC90YLRgCwg0JXQutCw0YLQtdGA0LjQvdCx0YPRgNCzLCDQodCy0LXRgNC00LvQvtCy0YHQutCw0Y8g0L7QsdC7Lg!5e1!3m2!1sru!2sru!4v1746274986905!5m2!1sru!2sru&ll=56.8386,60.6055&z=16"; 
      iframe.src = newSrc;
    });

    // app.js
document.addEventListener('DOMContentLoaded', function() {
  // JavaScript-код для инициализации Leaflet карты
  var map = L.map('map').setView([56.8386, 60.6055], 13); // Екатеринбург

  L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; OpenStreetMap'
  }).addTo(map);

  // Здесь можно добавлять маркеры и другой код для карты
});