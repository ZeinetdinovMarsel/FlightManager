import { useEffect, useState } from "react";
import { Routes, Route, Navigate, Link } from "react-router-dom";
import { useAuthStore } from "./services/authService";
import { AppBar, Toolbar, Typography, Button, Box, MenuItem, Select } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useSettingsStore } from "./store/settings";
import SignUpPage from "./pages/SignUpPage";
import SignInPage from "./pages/SignInPage";
import Profile from "./pages/Profile";
import AirportsPage from "./pages/AirportPage";
import FederalDistrictPage from "./pages/FederalDistrictsPage";
import FlightsPage from "./pages/FlightPage";
import ServicePage from "./pages/ServicePage";
import TicketPage from "./pages/TicketPage";


export default function App() {
  const settings = useSettingsStore();
  const { t } = useTranslation();
  const isAuthenticated = useAuthStore((state) => state.token);
  const signOut = useAuthStore((state) => state.signOut);
  const [language, setLanguage] = useState(settings.language);




  const handleSignOut = () => {
    signOut();
  };
  return (
    <div>

      <AppBar position="sticky">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            {t("siteName")}
          </Typography>


          <Box>
            {isAuthenticated ? (
              <>
                <Button color="inherit" component={Link} to="/flights">
                  {t("flights")}
                </Button>
                <Button color="inherit" component={Link} to="/airports">
                  {t("airports")}
                </Button>
                <Button color="inherit" component={Link} to="/federalDistricts">
                  {t("federalDistricts")}
                </Button>
                <Button color="inherit" component={Link} to="/tickets">
                  {t("tickets")}
                </Button>
                <Button color="inherit" component={Link} to="/services">
                  {t("services")}
                </Button>
                <Button color="inherit" component={Link} to="/profile">
                  {t("profile")}
                </Button>
                <Button color="inherit" onClick={handleSignOut}>
                  {t("signOut")}
                </Button>

              </>
            ) : (
              <>
                <Button color="inherit" component={Link} to="/signIn">
                  {t("signIn")}
                </Button>
                <Button color="inherit" component={Link} to="/signUp">
                  {t("signUp")}
                </Button>
              </>
            )}
          </Box>
        </Toolbar>
      </AppBar>


      <Routes>
        <Route path="/" element={<Navigate to="/profile" />} />
        <Route path="/signIn" element={!isAuthenticated ? <SignInPage /> : <Navigate to="/profile" />} />
        <Route path="/signUp" element={!isAuthenticated ? <SignUpPage /> : <Navigate to="/profile" />} />
        <Route path="/profile" element={isAuthenticated ? <Profile /> : <Navigate to="/signIn" />} />
        <Route path="/airports" element={isAuthenticated ? <AirportsPage /> : <Navigate to="/signIn" />} />
        <Route path="/federalDistricts" element={isAuthenticated ? <FederalDistrictPage /> : <Navigate to="/signIn" />} />
        <Route path="/flights" element={isAuthenticated ? <FlightsPage /> : <Navigate to="/signIn" />} />
        <Route path="/tickets" element={isAuthenticated ? <TicketPage /> : <Navigate to="/signIn" />} />
        <Route path="/services" element={isAuthenticated ? <ServicePage /> : <Navigate to="/signIn" />} />
      </Routes>
    </div>
  );
}
