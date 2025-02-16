import i18n from "i18next";
import { initReactI18next } from "react-i18next";

const resources = {
  en: {
    translation: {
      siteName: "Flight manager",
      signIn: "Sign in",
      signUp: "Sign up",
      signOut: "Sign out",
      authorization: "Authorization",
      registration: "Registration",
      email: "Email",
      username: "Username",
      password: "Password",
      dashboard: "Dashboard",
      profile: "Profile",
      users: "Users",
      orders: "Orders",
      table: "Table",
      welcome: "Welcome!",
      language: "Language",
      alreadyHaveAccount: "Already have account?",
      noAccount: "No account?",
    },
  },
  ru: {
    translation: {
      siteName: "Мэнеджер полётов",
      signIn: "Вход",
      signUp: "Зарегистрироваться",
      signOut: "Выйти",
      authorization: "Авторизация",
      registration: "Регистрация",
      email: "Эл. почта",
      username: "Имя пользователя",
      password: "Пароль",
      dashboard: "Главная",
      profile: "Профиль",
      users: "Пользователи",
      orders: "Заказы",
      table: "Таблица",
      welcome: "Добро пожаловать!",
      language: "Язык",
      alreadyHaveAccount: "Уже есть аккаунт?",
      noAccount: "Нет аккаунта?",
    },
  },
};

i18n.use(initReactI18next).init({
  resources,
  lng: "ru",
  fallbackLng: "en",
  interpolation: { escapeValue: false },
});

export default i18n;
