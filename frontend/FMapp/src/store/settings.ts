import { create } from "zustand";
import i18n from "../i18n";

interface SettingsState {
  language: "ru" | "en";
  setLanguage: (lang: "ru" | "en") => void;
  rowsPerPage: number;
  setRowsPerPage: (rows: number) => void;
  updateTime: number;
  setUpdateTime: (time: number) => void;
}
export const useSettingsStore = create<SettingsState>((set) => ({
  language: (localStorage.getItem("language") as "ru" | "en") || "ru",
  setLanguage: (lang) => {
    localStorage.setItem("language", lang);
    i18n.changeLanguage(lang);
    set({ language: lang });
  },
  rowsPerPage: parseInt(localStorage.getItem("rowsPerPage") || "5", 10),
  setRowsPerPage: (rows) => {
    localStorage.setItem("rowsPerPage", rows.toString());
    set({ rowsPerPage: rows });
  },
  updateTime: parseInt(localStorage.getItem("lastUpdateTime") || "5", 10),
  setUpdateTime: (time) => {
    localStorage.setItem("lastUpdateTime", time.toString());
    set({ updateTime: time });
  },

}));

