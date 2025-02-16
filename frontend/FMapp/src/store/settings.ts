import { create } from "zustand";

interface SettingsState {
  language: "ru" | "en";
  setLanguage: (lang: "ru" | "en") => void;
}

export const useSettingsStore = create<SettingsState>((set) => ({
  language: localStorage.getItem("language") as "ru" | "en" || "ru",
  setLanguage: (lang) => {
    localStorage.setItem("language", lang);
    set({ language: lang });
  },
}));
