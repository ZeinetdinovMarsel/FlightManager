import { useState } from "react";
import { Routes, Route, Navigate, Link } from "react-router-dom";
import { useAuthStore } from "./store/auth";
import { AppBar, Toolbar, Typography, Button, useMediaQuery, Box, IconButton, Menu, MenuItem, Select, FormControl, InputLabel } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import MenuIcon from "@mui/icons-material/Menu";
import { useTranslation } from "react-i18next";
import SignUpPage from "./pages/SignUpPage";
import SignInPage from "./pages/SignInPage";
import Dashboard from "./pages/Dashboard";
import Profile from "./pages/Profile";


export default function App() {
  const { t, i18n } = useTranslation();
  const isAuthenticated = useAuthStore((state) => state.token);
  const theme = useTheme();
  const isSmallScreen = useMediaQuery(theme.breakpoints.down("sm"));
  const signOut = useAuthStore((state) => state.signOut);

  const [anchorEl, setAnchorEl] = useState(null);
  const [language, setLanguage] = useState(i18n.language);

  const handleMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLanguageChange = (event) => {
    setLanguage(event.target.value);
    i18n.changeLanguage(event.target.value);
  };

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

          <Select
            value={language}
            onChange={handleLanguageChange}
            label={t("language")}
            sx={{
              color: "white",
              width: 120,
              maxWidth: "100%",
            }}
          >
            <MenuItem value="en">English</MenuItem>
            <MenuItem value="ru">Русский</MenuItem>
          </Select>

          {isSmallScreen ? (
            <Box>
              <IconButton color="inherit" onClick={handleMenuOpen}>
                <MenuIcon />
              </IconButton>
              <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
              >
                <MenuItem component={Link} to="/signIn" onClick={handleMenuClose}>
                  {t("signIn")}
                </MenuItem>
                {isAuthenticated && (
                  <>
                    <MenuItem component={Link} to="/dashboard" onClick={handleMenuClose}>
                      {t("dashboard")}
                    </MenuItem>
                    <MenuItem component={Link} to="/profile" onClick={handleMenuClose}>
                      {t("profile")}
                    </MenuItem>
                    <MenuItem onClick={handleSignOut}>
                      {t("signOut")}
                    </MenuItem>
                  </>
                )}
              </Menu>
            </Box>
          ) : (
            <Box>
              {isAuthenticated ? (
                <>
                  <Button color="inherit" component={Link} to="/dashboard">
                    {t("dashboard")}
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
          )}
        </Toolbar>
      </AppBar>

      
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" />}/>
        <Route path="/signIn" element={!isAuthenticated ? <SignInPage /> : <Navigate to="/dashboard" />} />
        <Route path="/signUp" element={!isAuthenticated ? <SignUpPage /> : <Navigate to="/" />} />
        <Route path="/dashboard" element={isAuthenticated ? <Dashboard /> : <Navigate to="/signIn" />} />
        <Route path="/profile" element={isAuthenticated ? <Profile /> : <Navigate to="/signIn" />} />
      </Routes>
    </div>
  );
}
