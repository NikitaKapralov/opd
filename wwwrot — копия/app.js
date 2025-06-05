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
  
    document.getElementById('registrationForm').addEventListener('submit', async function(event) {
      event.preventDefault();

      const username = document.getElementById('login').value.trim();
      const email = document.getElementById('email').value.trim();
      const password = document.getElementById('password').value;

      try {
        const response = await fetch('https://localhost:17182/api/users', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ username, email, password })
        });

        if (response.ok) {
          const data = await response.json();
          alert('Пользователь успешно создан!');
          window.location.href = 'index2.html';
        } else {
          const errorData = await response.json();
          alert('Ошибка: ' + (errorData.message || 'Не удалось создать пользователя'));
        }
      } catch (error) {
        alert('Ошибка сети: ' + error.message);
      }
    });
