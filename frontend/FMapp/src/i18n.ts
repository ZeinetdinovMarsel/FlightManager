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
      languageName: "English",
      alreadyHaveAccount: "Already have account?",
      noAccount: "No account?",
      flights: "Flights",
      flightsManagement: "Flights Management",
      airports: "Airports",
      airportsManagement: "Airports Management",
      actions: "Actions",
      edit: "Edit",
      delete: "Delete",
      addAirport: "Add Airport",
      airportName: "Airport Name",
      city: "City",
      country: "Country",
      confirmDelete: "Are you sure you want to delete this?",
      save: "Save",
      cancel: "Cancel",
      noData: "No data available",
    },
  },
  ru: {
    translation: {
      siteName: "Менеджер полётов",
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
      languageName: "Русский",
      alreadyHaveAccount: "Уже есть аккаунт?",
      noAccount: "Нет аккаунта?",
      flights: "Рейсы",
      flightsManagement: "Управление рейсами",
      airports: "Аэропорты",
      airportsManagement: "Управление аэропортами",
      actions: "Действия",
      edit: "Редактировать",
      delete: "Удалить",
      addAirport: "Добавить аэропорт",
      airportName: "Название аэропорта",
      city: "Город",
      country: "Страна",
      confirmDelete: "Вы уверены, что хотите это удалить?",
      save: "Сохранить",
      cancel: "Отмена",
      noData: "Нет доступных данных",
    },
  },
};
const storedLanguage = localStorage.getItem("language") || "ru";

i18n
  .use(initReactI18next)
  .init({
    resources,
    lng: storedLanguage,
    fallbackLng: "en",
    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
