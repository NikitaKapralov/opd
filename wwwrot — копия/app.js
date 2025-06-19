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
  async function loadVisitedLocations() {
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
        displayVisitedLocations(userData.visitedLocations || []);
      } else {
        console.error('Ошибка при загрузке истории:', response.status);
        window.location.href = 'index2.html';
      }
    } catch (error) {
      console.error('Ошибка сети:', error);
      window.location.href = 'index2.html';
    }
  }

  // Функция для отображения посещенных мест
  function displayVisitedLocations(locations) {
    const container = document.getElementById('visitedLocations');
    if (!container) return;

    // Очищаем контейнер
    container.innerHTML = '';

    if (locations.length === 0) {
      container.innerHTML = '<p>Вы еще не посещали ни одного места.</p>';
      return;
    }

    // Отображаем места в обратном порядке (последние посещенные - первыми)
    locations.reverse().forEach(location => {
      const locationElement = document.createElement('div');
      locationElement.className = 'location-item';
      
      locationElement.innerHTML = `
        <div>
          <span class="location-name">${location.name || 'Неизвестное место'}</span>
          <div class="location-date">Посещено: ${new Date(location.visitedAt).toLocaleString()}</div>
        </div>
        <a href="${location.url || 'index6.html'}" class="visit-again">Посмотреть</a>
      `;
      
      container.appendChild(locationElement);
    });
  }

  if (document.querySelector('h1')?.textContent === 'История посещений') {
    loadVisitedLocations();
  }
  async function loadLeaderboard() {
  try {
    const userId = localStorage.getItem('userId');
    const response = await fetch('https://localhost:17182/api/users', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('authToken')}`
      }
    });
    
    if (response.ok) {
      const users = await response.json();
      displayLeaderboard(users, userId);
    } else {
      console.error('Ошибка при загрузке рейтинга:', response.status);
      if (response.status === 401) {
        window.location.href = 'index2.html';
      }
    }
  } catch (error) {
    console.error('Ошибка сети:', error);
    window.location.href = 'index2.html';
  }
}

function displayLeaderboard(users, currentUserId) {
  const leaderboardContainer = document.getElementById('leaderboard');
  const currentUserContainer = document.getElementById('currentUserStats');
  
  if (!leaderboardContainer || !currentUserContainer) return;

  // Сортируем пользователей по количеству посещенных мест (по убыванию)
  const sortedUsers = [...users].sort((a, b) => {
    const aCount = a.visitedLocations?.length || 0;
    const bCount = b.visitedLocations?.length || 0;
    return bCount - aCount;
  });

  // Очищаем контейнеры
  leaderboardContainer.innerHTML = '';
  currentUserContainer.innerHTML = '';

  // Находим текущего пользователя
  const currentUser = sortedUsers.find(user => user.id == currentUserId);
  
  // Отображаем текущего пользователя
  if (currentUser) {
    const currentUserPosition = sortedUsers.findIndex(user => user.id == currentUserId) + 1;
    const visitedCount = currentUser.visitedLocations?.length || 0;
    
    currentUserContainer.innerHTML = `
      <div class="current-user-stats">
        <img src="${localStorage.getItem('userAvatar') || '123.png'}" alt="Аватар" class="current-user-avatar">
        <div class="current-user-details">
          <div class="current-user-name">${currentUser.username}</div>
          <div class="current-user-level">Уровень: ${currentUser.level || 1}</div>
          <div class="current-user-score">Посещено мест: ${visitedCount} (${currentUserPosition} место)</div>
        </div>
      </div>
    `;
  }

  // Отображаем таблицу лидеров
  if (sortedUsers.length === 0) {
    leaderboardContainer.innerHTML = '<p>Нет данных о пользователях</p>';
    return;
  }

  sortedUsers.forEach((user, index) => {
    const visitedCount = user.visitedLocations?.length || 0;
    const isCurrentUser = user.id == currentUserId;
    
    const userRow = document.createElement('div');
    userRow.className = `user-row ${isCurrentUser ? 'current-user' : ''}`;
    
    userRow.innerHTML = `
      <div class="user-position">${index + 1}</div>
      <img src="${user.avatarUrl || '123.png'}" alt="Аватар" class="user-avatar">
      <div class="user-info">
        <div class="user-name">${user.username}</div>
        <div class="user-level">Уровень ${user.level || 1}</div>
      </div>
      <div class="user-score">${visitedCount}</div>
    `;
    
    leaderboardContainer.appendChild(userRow);
  });
}

// Проверяем, находимся ли мы на странице рейтинга
if (document.querySelector('h1')?.textContent === 'Ваши баллы и рейтинг') {
  loadLeaderboard();
}
// Функция для загрузки аватарки пользователя
async function loadUserAvatar() {
  try {
    const userId = localStorage.getItem('userId');
    if (!userId) return;

    const response = await fetch(`https://localhost:17182/api/users/${userId}/avatar`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('authToken')}`
      }
    });

    if (response.ok) {
      const blob = await response.blob();
      const imageUrl = URL.createObjectURL(blob);
      updateProfileImages(imageUrl);
      // Сохраняем временный URL в localStorage для использования до перезагрузки страницы
      localStorage.setItem('tempAvatarUrl', imageUrl);
    } else if (response.status !== 404) {
      console.error('Ошибка при загрузке аватарки:', response.status);
    }
  } catch (error) {
    console.error('Ошибка сети при загрузке аватарки:', error);
  }
}

// Функция для обновления всех изображений профиля
function updateProfileImages(imageUrl) {
  const profileImages = document.querySelectorAll('.profile-image');
  profileImages.forEach(img => {
    img.onload = function() {
      // Освобождаем предыдущий объект URL, если он существует
      if (img.dataset.objectUrl) {
        URL.revokeObjectURL(img.dataset.objectUrl);
      }
      img.dataset.objectUrl = imageUrl;
    };
    img.src = imageUrl;
  });
}

// Обработчик изменения аватарки
document.getElementById('avatar')?.addEventListener('change', async function() {
  const file = this.files[0];
  if (!file) return;

  if (!file.type.startsWith('image/')) {
    alert('Пожалуйста, выберите файл изображения');
    return;
  }

  try {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      window.location.href = 'index2.html';
      return;
    }

    const formData = new FormData();
    formData.append('avatar', file);

    // Показываем превью перед загрузкой
    const previewUrl = URL.createObjectURL(file);
    updateProfileImages(previewUrl);

    const response = await fetch(`https://localhost:17182/api/users/${userId}/avatar`, {
      method: 'PATCH',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('authToken')}`
      },
      body: formData
    });

    if (response.ok) {
      const blob = await response.blob();
      const imageUrl = URL.createObjectURL(blob);
      updateProfileImages(imageUrl);
      localStorage.setItem('tempAvatarUrl', imageUrl);
      alert('Аватар успешно обновлен!');
    } else {
      // В случае ошибки возвращаем предыдущий аватар
      const prevAvatar = localStorage.getItem('tempAvatarUrl') || '123.png';
      updateProfileImages(prevAvatar);
      const errorData = await response.json();
      alert('Ошибка при обновлении аватара: ' + (errorData.message || 'Неизвестная ошибка'));
    }
  } catch (error) {
    console.error('Ошибка сети:', error);
    const prevAvatar = localStorage.getItem('tempAvatarUrl') || '123.png';
    updateProfileImages(prevAvatar);
    alert('Произошла ошибка при обновлении аватара');
  } finally {
    this.value = ''; // Сбрасываем значение input
  }
});

// При загрузке страницы проверяем временный URL
document.addEventListener('DOMContentLoaded', () => {
  const savedTempUrl = localStorage.getItem('tempAvatarUrl');
  if (savedTempUrl) {
    // Проверяем, что URL еще действителен
    const img = new Image();
    img.onload = function() {
      updateProfileImages(savedTempUrl);
    };
    img.onerror = function() {
      localStorage.removeItem('tempAvatarUrl');
      loadUserAvatar(); // Загружаем аватар с сервера
    };
    img.src = savedTempUrl;
  } else {
    loadUserAvatar();
  }
});
});