import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { TextField, Button, Grid, Box, Typography } from "@mui/material";
import { signUpUser } from "../services/authService";

export default function SignUpPage() {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string>("");

  const handleSignUp = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      const data = await signUpUser(userName, email, password, 1);
      console.log(data);
      navigate("/signIn");
    } catch (err: any) {
      setError(err.message);
    }
  };

  return (
    <Box sx={{ maxWidth: 400, margin: "auto", padding: 3 }}>
      <Typography variant="h4" align="center" gutterBottom>
        {t("registration")}
      </Typography>

      <form onSubmit={handleSignUp}>
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label={t("username")}
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              required
            />
          </Grid>

          <Grid item xs={12}>
            <TextField
              fullWidth
              type="email"
              label={t("email")}
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </Grid>

          <Grid item xs={12}>
            <TextField
              fullWidth
              type="password"
              label={t("password")}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </Grid>

          {error && (
            <Grid item xs={12}>
              <Typography color="error" align="center">{error}</Typography>
            </Grid>
          )}

          <Grid item xs={12}>
            <Button fullWidth variant="contained" color="primary" type="submit">{t("signUp")}</Button>
          </Grid>
          <Grid item xs={12}>
            <Typography align="center">
              {t("alreadyHaveAccount")} <Link to="/signIn">{t("signIn")}</Link>
            </Typography>
          </Grid>
        </Grid>
      </form>
    </Box>
  );
}
