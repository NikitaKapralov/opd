document.addEventListener('DOMContentLoaded', function() {
 async function loadUserProfile() {
    try {
      const userId = localStorage.getItem('userId');
      
      if (!userId) {
        console.log('Пользователь не авторизован');
        window.location.href = 'index2.html';
        return;
      }

      const response = await fetch(`https://localhost:17182/api/users/${userId}`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('authToken')}`
        }
      });
      
      if (response.ok) {
        const userData = await response.json();
        document.getElementById('login').value = userData.username || '';
        document.getElementById('email').value = userData.email || '';
      } else {
        console.error('Ошибка при загрузке профиля:', response.status);
        window.location.href = 'index2.html';
      }
    } catch (error) {
      console.error('Ошибка сети:', error);
      window.location.href = 'index2.html';
    }
  }

function updateAuthUI() {
  const isLoggedIn = !!localStorage.getItem('authToken');
  
  const profileLinks = document.querySelectorAll('a[href="index3.html"]');
  profileLinks.forEach(link => {
    link.style.display = isLoggedIn ? 'block' : 'none';
  });
  
  const authLinks = document.querySelectorAll('a.auth-link, a[href="index2.html"]:not(.auth-link)');
  authLinks.forEach(link => {
    if (link.closest('.registration-form')) {
      link.style.display = 'inline';
    } else {
      link.style.display = isLoggedIn ? 'none' : 'block';
    }
  });
  
  const logoutBtn = document.getElementById('logoutBtn');
  if (logoutBtn) {
    logoutBtn.style.display = isLoggedIn ? 'block' : 'none';
  }
}

   document.getElementById('logoutBtn')?.addEventListener('click', () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userId');
    localStorage.removeItem('username');
    localStorage.removeItem('email');
    window.location.href = 'index2.html';
  });

  const themeToggle = document.getElementById('themeToggle');
  if (themeToggle) {
    themeToggle.addEventListener('click', function() {
      const body = document.body;
      if (body.classList.contains('light-theme')) {
        body.classList.replace('light-theme', 'dark-theme');
        localStorage.setItem('theme', 'dark');
      } else if (body.classList.contains('dark-theme')) {
        body.classList.replace('dark-theme', 'light-theme');
        localStorage.setItem('theme', 'light');
      } else {
        body.classList.add('dark-theme');
        localStorage.setItem('theme', 'dark');
      }
    });
  }

  const savedTheme = localStorage.getItem('theme');
  if (savedTheme === 'dark') {
    document.body.classList.add('dark-theme');
  } else {
    document.body.classList.add('light-theme');
  }

  const loginForm = document.getElementById('loginForm');
  if (loginForm) {
    loginForm.addEventListener('submit', async function(event) {
      event.preventDefault();

      const username = document.getElementById('login').value.trim();
      const password = document.getElementById('password').value;

      try {
        const response = await fetch('https://localhost:17182/api/users/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password })
        });

        if (response.ok) {
          const data = await response.json();
          localStorage.setItem('authToken', data.token);
          localStorage.setItem('userId', data.id);
          localStorage.setItem('username', data.username);
          localStorage.setItem('email', data.email);
          window.location.href = 'index3.html';
        } else {
          const errorData = await response.json();
          alert('Ошибка входа: ' + (errorData.message || 'Неверные данные'));
        }
      } catch (error) {
        alert('Ошибка сети: ' + error.message);
      }
    });
  }

  const registrationForm = document.getElementById('registrationForm');
  if (registrationForm) {
    registrationForm.addEventListener('submit', async function(event) {
      event.preventDefault();

      const username = document.getElementById('login').value.trim();
      const email = document.getElementById('email') ? document.getElementById('email').value.trim() : '';
      const password = document.getElementById('password').value;

      try {
        const response = await fetch('https://localhost:17182/api/users', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
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
  }

  if (document.querySelector('.registration-form h1')?.textContent === 'Профиль') {
    loadUserProfile();
  }

  updateAuthUI();
    document.getElementById('saveChanges')?.addEventListener('click', async function() {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      alert('Пользователь не авторизован');
      window.location.href = 'index2.html';
      return;
    }

    const oldPassword = document.getElementById('old-password').value;
    const newPassword = document.getElementById('new-password').value;
    const confirmPassword = document.getElementById('confirm-password').value;

    // Валидация полей
    if (!oldPassword || !newPassword || !confirmPassword) {
      alert('Все поля должны быть заполнены');
      return;
    }

    if (newPassword !== confirmPassword) {
      alert('Новый пароль и подтверждение не совпадают');
      return;
    }

    if (newPassword.length < 6) {
      alert('Пароль должен содержать минимум 6 символов');
      return;
    }

    try {
      const response = await fetch(`https://localhost:17182/api/users/${userId}/password`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('authToken')}`
        },
        body: JSON.stringify({
          currentPassword: oldPassword,
          newPassword: newPassword
        })
      });

      if (response.ok) {
        alert('Пароль успешно изменён');
        // Очищаем поля после успешного изменения
        document.getElementById('old-password').value = '';
        document.getElementById('new-password').value = '';
        document.getElementById('confirm-password').value = '';
      } else {
        const errorData = await response.json();
        alert('Ошибка при изменении пароля: ' + (errorData.message || 'Неизвестная ошибка'));
      }
    } catch (error) {
      console.error('Ошибка сети:', error);
      alert('Произошла ошибка при изменении пароля');
    }
  });

});