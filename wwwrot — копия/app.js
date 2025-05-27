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
  


    