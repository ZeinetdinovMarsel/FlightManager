import { useAuthStore } from "../store/auth";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import LanguageSwitcher from "../components/LanguageSwitcher";
import Typography from "@mui/material/Typography/Typography";

export default function Dashboard() {
  const { t } = useTranslation();


  return (
    <div>
      <Typography variant="h1" align="center" gutterBottom>
      {t("welcome")}
      </Typography>
      <Typography variant="h1" align="center" gutterBottom>
      Лабораторная работа №2
      </Typography>
    </div>
  );
}
