document.addEventListener('DOMContentLoaded', function() {
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

      if (!username || !password) {
        alert('Пожалуйста, заполните все поля');
        return;
      }

      try {
        const response = await fetch('https://localhost:17182/api/users/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password })
        });

        if (response.ok) {
          const data = await response.json();
          localStorage.setItem('currentUserId', data.id);
          localStorage.setItem('username', data.username);
          if (data.avatarUrl) {
            localStorage.setItem('avatarUrl', data.avatarUrl);
          }
          
          alert('Вход выполнен успешно!');
          window.location.href = 'index3.html'; 
        } else {
          const errorData = await response.json();
          alert('Ошибка входа: ' + (errorData.message || 'Не удалось войти'));
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
  
  async function updateAvatar(userId, file) {
    const formData = new FormData();
    formData.append('avatarFile', file);
    
    const response = await fetch(`/api/users/${userId}/avatar`, {
      method: 'PATCH',
      body: formData
    });
    
    return response.json();
  }

  const avatarInput = document.getElementById('avatarInput');
  const changeAvatarBtn = document.getElementById('changeAvatarBtn');
  const saveAvatarBtn = document.getElementById('saveAvatarBtn');
  const cancelAvatarBtn = document.getElementById('cancelAvatarBtn');
  const profileAvatar = document.getElementById('profileAvatar');
  const headerAvatar = document.getElementById('headerAvatar');
  
  let selectedFile = null;

  const savedAvatarUrl = localStorage.getItem('avatarUrl');
  if (savedAvatarUrl) {
    profileAvatar.src = savedAvatarUrl;
    if (headerAvatar) headerAvatar.src = savedAvatarUrl;
  }

  changeAvatarBtn.addEventListener('click', () => {
    avatarInput.click();
  });

  avatarInput.addEventListener('change', (event) => {
    if (event.target.files && event.target.files[0]) {
      selectedFile = event.target.files[0];
      const reader = new FileReader();
      
      reader.onload = (e) => {
        profileAvatar.src = e.target.result;
      };
      
      reader.readAsDataURL(selectedFile);
      
      changeAvatarBtn.style.display = 'none';
      saveAvatarBtn.style.display = 'inline-block';
      cancelAvatarBtn.style.display = 'inline-block';
    }
  });

  saveAvatarBtn.addEventListener('click', async () => {
    if (selectedFile) {
      try {
        const userId = localStorage.getItem('currentUserId');
        if (!userId) {
          alert('Пользователь не авторизован');
          return;
        }
        
        const result = await updateAvatar(userId, selectedFile);
        
        if (result.success) {
          const newAvatarUrl = result.avatarUrl || URL.createObjectURL(selectedFile);
          profileAvatar.src = newAvatarUrl;
          if (headerAvatar) headerAvatar.src = newAvatarUrl;
          localStorage.setItem('avatarUrl', newAvatarUrl);
          
          alert('Аватар успешно обновлен!');
        } else {
          alert('Ошибка при обновлении аватара');
        }
      } catch (error) {
        console.error('Ошибка:', error);
        alert('Произошла ошибка при обновлении аватара');
      }
    }
    
    resetAvatarButtons();
  });

  cancelAvatarBtn.addEventListener('click', () => {
    const savedAvatar = localStorage.getItem('avatarUrl');
    profileAvatar.src = savedAvatar || '123.png';
    resetAvatarButtons();
  });

  function resetAvatarButtons() {
    changeAvatarBtn.style.display = 'inline-block';
    saveAvatarBtn.style.display = 'none';
    cancelAvatarBtn.style.display = 'none';
    selectedFile = null;
    avatarInput.value = '';
  }

  document.getElementById('saveChangesBtn').addEventListener('click', () => {
    alert('Изменения сохранены!');
  });
});
