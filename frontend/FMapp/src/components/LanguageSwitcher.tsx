import { useTranslation } from "react-i18next";
import { useSettingsStore } from "../store/settings";

export default function LanguageSwitcher() {
  const { i18n } = useTranslation();
  const { language, setLanguage } = useSettingsStore();

  const toggleLanguage = () => {
    const newLang = language === "ru" ? "en" : "ru";
    setLanguage(newLang);
    i18n.changeLanguage(newLang);
  };

  return <button onClick={toggleLanguage}>{language === "ru" ? "EN" : "RU"}</button>;
}
