import React, { useState } from "react";
import { TextField, Button, Typography, Container, Box, Grid } from "@mui/material";
import { signInUser } from "../services/authService";
import { useAuthStore } from "../store/auth";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";


export default function SignInPage() {
  const { t } = useTranslation();
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string>("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const { token, refreshToken } = await signInUser(email, password);
      useAuthStore.getState().signIn(token, refreshToken);
    } catch (err: any) {
      setError(err.message);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box sx={{ display: "flex", flexDirection: "column", alignItems: "center", marginTop: 8 }}>
        <Typography variant="h4" component="h1">
          {t("authorization")}
        </Typography>
        <form onSubmit={handleSubmit} style={{ width: "100%", marginTop: 8 }}>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <TextField
                label={t("email")}
                variant="outlined"
                fullWidth
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                label={t("password")}
                type="password"
                variant="outlined"
                fullWidth
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </Grid>
          
          {error && <Typography color="error" variant="body2" sx={{ marginTop: 2 }}>{error}</Typography>}
          <Grid item xs={12}>
            <Button fullWidth variant="contained" color="primary" type="submit">{t("signIn")}</Button>
          </Grid>
          <Grid item xs={12}>
            <Typography align="center">
              {t("noAccount")} <Link to="/signUp">{t("signUp")}</Link>
            </Typography>
          </Grid>
          </Grid>
        </form>
      </Box>
    </Container>
  );
};
