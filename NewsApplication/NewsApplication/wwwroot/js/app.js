// wwwroot/js/app.js
// Глобальные переменные
// wwwroot/js/app.js (дополнение в начало)
// Проверяем время сессии
function checkSession() {
    const loginTime = localStorage.getItem('loginTime');
    if (!loginTime) {
        logout();
        return;
    }

    const loginDate = new Date(loginTime);
    const now = new Date();
    const diffHours = (now - loginDate) / (1000 * 60 * 60);

    // Сессия истекает через 8 часов
    if (diffHours > 8) {
        alert('Сессия истекла. Пожалуйста, войдите снова.');
        logout();
    }
}

// В функции checkAuth добавь проверку сессии:
function checkAuth() {
    // Сначала проверяем сессию
    checkSession();

    const canView = localStorage.getItem('canView') === 'true';
    const username = localStorage.getItem('username');
    const userId = localStorage.getItem('userId');
    const role = localStorage.getItem('userRole');

    if (!canView || !username) {
        window.location.href = "login.html";
    } else {
        currentUser = {
            id: parseInt(userId),
            username: username,
            role: role,
            canEdit: localStorage.getItem('canEdit') === 'true',
            canDelete: localStorage.getItem('canDelete') === 'true'
        };

        updateUserInfo();
        setupUI();
        loadCategories();
        loadArticles();
    }
}
let currentUser = null;
let categories = [];
let articles = [];

// Проверяем авторизацию при загрузке страницы
document.addEventListener('DOMContentLoaded', function () {
    checkAuth();
});

function checkAuth() {
    const canView = localStorage.getItem('canView') === 'true';
    const username = localStorage.getItem('username');
    const userId = localStorage.getItem('userId');
    const role = localStorage.getItem('userRole');

    if (!canView || !username) {
        window.location.href = "login.html";
    } else {
        currentUser = {
            id: parseInt(userId),
            username: username,
            role: role,
            canEdit: localStorage.getItem('canEdit') === 'true',
            canDelete: localStorage.getItem('canDelete') === 'true'
        };

        updateUserInfo();
        setupUI();
        loadCategories();
        loadArticles();
    }
}

function updateUserInfo() {
    document.getElementById('user-greeting').innerHTML = `
        👤 <strong>${currentUser.username}</strong>
        <span style="color: #666;">(${currentUser.role})</span>
    `;
}

function setupUI() {
    // Показываем/скрываем кнопку создания
    const createBtnContainer = document.getElementById('create-button-container');
    if (!currentUser.canEdit) {
        createBtnContainer.style.display = 'none';
    }
}

async function loadCategories() {
    try {
        const response = await fetch('https://localhost:7036/api/Categories');
        if (response.ok) {
            categories = await response.json();
            updateCategorySelect();
        }
    } catch (error) {
        console.error('Ошибка загрузки категорий:', error);
    }
}

async function loadArticles() {
    try {
        document.getElementById('loading').style.display = 'block';
        const response = await fetch('https://localhost:7036/api/Articles');
        if (response.ok) {
            articles = await response.json();
            renderArticles();
        }
    } catch (error) {
        console.error('Ошибка загрузки статей:', error);
        showMessage('Ошибка загрузки статей', 'error');
    } finally {
        document.getElementById('loading').style.display = 'none';
    }
}

function renderArticles() {
    const container = document.getElementById('articles-container');

    if (!articles || articles.length === 0) {
        container.innerHTML = '<div style="text-align: center; padding: 40px; color: #666;">Нет статей для отображения</div>';
        return;
    }

    container.innerHTML = articles.map(article => `
        <div class="article-card">
            ${article.cover_image_url ? `
                <img src="${article.cover_image_url}" alt="${article.title}" class="article-image">
            ` : `<div class="article-image" style="background: #eee; display: flex; align-items: center; justify-content: center; color: #999;">Нет изображения</div>`}
            <div class="article-content">
                <h3 class="article-title">${article.title || 'Без названия'}</h3>
                <p class="article-excerpt">${article.excerpt || (article.content ? article.content.substring(0, 150) + '...' : 'Нет содержания')}</p>

                <div class="article-meta">
                    <div>
                        📅 ${article.published ? new Date(article.published).toLocaleDateString('ru-RU') : 'Не указана'}
                    </div>
                    <div>
                        👤 ${article.author ? article.author.username : 'Неизвестный автор'}
                    </div>
                    <div>
                        🏷️ ${article.category ? article.category.name : 'Без категории'}
                    </div>
                </div>

                <div class="article-actions">
                    <button onclick="viewArticle(${article.id})" class="view-btn">Читать</button>
                    
                    ${currentUser.canEdit && (currentUser.role === 'Admin' || article.author_id === currentUser.id) ? `
                        <button onclick="editArticle(${article.id})" class="edit-btn">Редактировать</button>
                    ` : ''}
                    
                    ${currentUser.canDelete && (currentUser.role === 'Admin' || article.author_id === currentUser.id) ? `
                        <button onclick="deleteArticle(${article.id})" class="delete-btn">Удалить</button>
                    ` : ''}
                </div>
            </div>
        </div>
    `).join('');
}

function updateCategorySelect() {
    const select = document.getElementById('category_id');
    if (!select) return;

    select.innerHTML = '<option value="">Выберите категорию</option>' +
        categories.map(cat => `<option value="${cat.id}">${cat.name}</option>`).join('');
}

function showCreateForm() {
    document.getElementById('form-title').textContent = 'Создать новую статью';
    document.getElementById('articleForm').reset();
    document.getElementById('articleId').value = '';
    document.getElementById('article-form-container').style.display = 'block';
    document.getElementById('create-button-container').style.display = 'none';
    document.getElementById('message').style.display = 'none';
    window.scrollTo(0, 0);
}

function hideForm() {
    document.getElementById('article-form-container').style.display = 'none';
    document.getElementById('create-button-container').style.display = 'block';
    document.getElementById('message').style.display = 'none';
}

async function editArticle(id) {
    const article = articles.find(a => a.id === id);
    if (!article) return;

    document.getElementById('form-title').textContent = 'Редактировать статью';
    document.getElementById('articleId').value = article.id;
    document.getElementById('title').value = article.title;
    document.getElementById('excerpt').value = article.excerpt || '';
    document.getElementById('content').value = article.content;
    document.getElementById('cover_image_url').value = article.cover_image_url || '';
    document.getElementById('category_id').value = article.category_id;

    document.getElementById('article-form-container').style.display = 'block';
    document.getElementById('create-button-container').style.display = 'none';
    window.scrollTo(0, 0);
}

function viewArticle(id) {
    // Можно реализовать отдельную страницу для просмотра
    alert(`Просмотр статьи ${id}\nМожно реализовать отдельную страницу для детального просмотра`);
}

// Обработка формы
document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('articleForm');
    if (form) {
        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            const articleId = document.getElementById('articleId').value;
            const isEdit = !!articleId;

            const articleData = {
                title: document.getElementById('title').value,
                excerpt: document.getElementById('excerpt').value,
                content: document.getElementById('content').value,
                cover_image_url: document.getElementById('cover_image_url').value,
                category_id: parseInt(document.getElementById('category_id').value),
                author_id: currentUser.id
            };

            try {
                let response;
                if (isEdit) {
                    articleData.id = parseInt(articleId);
                    response = await fetch(`https://localhost:7036/api/Articles/${articleId}`, {
                        method: 'PUT',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(articleData)
                    });
                } else {
                    response = await fetch('https://localhost:7036/api/Articles', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(articleData)
                    });
                }

                if (response.ok) {
                    showMessage('Статья успешно сохранена!', 'success');
                    await loadArticles();
                    setTimeout(hideForm, 1500);
                } else {
                    const error = await response.text();
                    showMessage(`Ошибка: ${error}`, 'error');
                }
            } catch (error) {
                showMessage('Ошибка сети: ' + error.message, 'error');
            }
        });
    }
});

async function deleteArticle(id) {
    if (!confirm('Вы уверены, что хотите удалить эту статью?')) return;

    try {
        const response = await fetch(`https://localhost:7036/api/Articles/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showMessage('Статья удалена!', 'success');
            await loadArticles();
        } else {
            showMessage('Ошибка удаления', 'error');
        }
    } catch (error) {
        showMessage('Ошибка сети', 'error');
    }
}

function showMessage(text, type) {
    const messageDiv = document.getElementById('message');
    if (!messageDiv) return;

    messageDiv.textContent = text;
    messageDiv.className = `message ${type}`;
    messageDiv.style.display = 'block';

    if (type === 'success') {
        setTimeout(() => {
            messageDiv.style.display = 'none';
        }, 3000);
    }
}

function logout() {
    localStorage.clear();
    window.location.href = "login.html";
}

// Экспортируем функции для использования в консоли (опционально)
window.app = {
    currentUser,
    articles,
    categories,
    loadArticles,
    loadCategories,
    logout,
    showCreateForm,
    hideForm
};