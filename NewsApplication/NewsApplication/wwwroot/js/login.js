// wwwroot/js/login.js
document.addEventListener('DOMContentLoaded', function () {
    // Проверяем, не авторизован ли пользователь уже
    const isLoggedIn = localStorage.getItem('canView') === 'true';
    if (isLoggedIn) {
        window.location.href = "index.html";
    }

    // Назначаем обработчики событий
    setupEventListeners();
});

function setupEventListeners() {
    // Обработчик кнопки входа
    const loginButton = document.querySelector('button');
    if (loginButton) {
        loginButton.addEventListener('click', login);
    }

    // Обработчик нажатия Enter в поле пароля
    const passwordInput = document.getElementById('password');
    if (passwordInput) {
        passwordInput.addEventListener('keypress', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                login();
            }
        });
    }

    // Обработчик нажатия Enter в поле имени пользователя
    const usernameInput = document.getElementById('username');
    if (usernameInput) {
        usernameInput.addEventListener('keypress', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                document.getElementById('password').focus();
            }
        });
    }
}

async function login() {
    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value;
    const errorElement = document.getElementById("errorMessage");
    const loadingElement = document.getElementById("loading");

    // Валидация
    if (!username || !password) {
        errorElement.textContent = "Пожалуйста, заполните все поля";
        return;
    }

    errorElement.textContent = "";
    loadingElement.style.display = "block";

    try {
        // Отправляем запрос на бэкенд
        const response = await fetch('https://localhost:7036/api/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Username: username,
                Password: password
            })
        });

        if (response.ok) {
            const userData = await response.json();

            // Сохраняем данные пользователя
            saveUserData(userData);

            // Перенаправляем на главную
            window.location.href = "index.html";
        } else {
            const error = await response.json();
            errorElement.textContent = error.message || "Неверные учетные данные";
        }
    } catch (error) {
        console.error("Ошибка:", error);
        errorElement.textContent = "Ошибка подключения к серверу";

        // Фолбэк: проверяем хардкод пользователей если API недоступен
        checkHardcodedUsers(username, password, errorElement);
    } finally {
        loadingElement.style.display = "none";
    }
}

function saveUserData(userData) {
    localStorage.setItem("userId", userData.id);
    localStorage.setItem("username", userData.username);
    localStorage.setItem("userRole", userData.role);
    localStorage.setItem("canView", userData.canView);
    localStorage.setItem("canEdit", userData.canEdit);
    localStorage.setItem("canDelete", userData.canDelete);
    localStorage.setItem("email", userData.email || "");

    // Также можно сохранить время входа
    localStorage.setItem("loginTime", new Date().toISOString());
}

function checkHardcodedUsers(username, password, errorElement) {
    // Фолбэк на случай если API не работает
    if (username === "Nick" && password === "Nick123") {
        saveUserData({
            id: 1,
            username: "Nick",
            role: "Author",
            canView: true,
            canEdit: true,
            canDelete: false,
            email: "nick@example.com"
        });
        window.location.href = "index.html";
    }
    else if (username === "Joe" && password === "Joe123") {
        saveUserData({
            id: 2,
            username: "Joe",
            role: "Reader",
            canView: true,
            canEdit: false,
            canDelete: false,
            email: "joe@example.com"
        });
        window.location.href = "index.html";
    }
    else if (username === "Editor" && password === "Editor123") {
        saveUserData({
            id: 3,
            username: "Editor",
            role: "Admin",
            canView: true,
            canEdit: true,
            canDelete: true,
            email: "editor@example.com"
        });
        window.location.href = "index.html";
    } else {
        errorElement.textContent = "Неверные учетные данные";
    }
}

// Экспортируем функцию для тестирования (опционально)
window.loginModule = {
    login,
    saveUserData,
    checkHardcodedUsers
};