import { create } from "zustand";
import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

interface AuthState {
  token: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  signIn: (token: string, refreshToken: string, expiresIn: number) => void;
  signOut: () => void;
  refreshAccessToken: () => Promise<string | null>;
}

export const useAuthStore = create<AuthState>((set, get) => {
  let refreshTimeout: NodeJS.Timeout;


  const scheduleTokenRefresh = (expiresIn: number) => {
    clearTimeout(refreshTimeout);
    refreshTimeout = setTimeout(() => {
      get().refreshAccessToken();

    }, (expiresIn - 30) * 1000);
  };

  return {
    token: localStorage.getItem("token"),
    refreshToken: localStorage.getItem("refreshToken"),
    isAuthenticated: !!localStorage.getItem("token"),

    signIn: (token, refreshToken, expiresIn) => {
      localStorage.setItem("token", token);
      localStorage.setItem("refreshToken", refreshToken);
      set({ token, refreshToken, isAuthenticated: true });

      scheduleTokenRefresh(expiresIn);
    },

    signOut: () => {
      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      set({ token: null, refreshToken: null, isAuthenticated: false });
    },

    refreshAccessToken: async () => {
      try {
        const refreshToken = get().refreshToken;
        if (!refreshToken) {
          get().signOut();
          return null;
        }

        const response = await axiosInstance.post(`${API_URL}refreshToken`, { refreshToken });
        const { accessToken, refreshToken: newRefreshToken, expiresIn } = response.data;

        localStorage.setItem("token", accessToken);
        localStorage.setItem("refreshToken", newRefreshToken);

        set({ token: accessToken, refreshToken: newRefreshToken, isAuthenticated: true });

        scheduleTokenRefresh(expiresIn);

        return accessToken;
      } catch (error) {
        get().signOut();
        return null;
      }
    },
  };
});
