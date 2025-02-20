// src/components/UserProfile.tsx
import React, { useEffect, useRef, useState } from "react";
import { useSettingsStore } from "../store/settings";
import {
  Container,
  Typography,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  Grid,
  Box,
  Alert,
} from "@mui/material";
import axiosInstance from "../utils/axiosInstance";
import { useTranslation } from "react-i18next";


const UserProfile: React.FC = () => {
  const { t } = useTranslation();
  const settings = useSettingsStore();
  const [language, setLanguage] = useState<string>("");
  const [rowsPerPage, setRowsPerPage] = useState<number>(0);
  const [updateTime, setUpdateTime] = useState<number>(0);
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [userid, setUserId] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);


  const initialUsername = useRef<string>('');
  const initialEmail = useRef<string>('');
  useEffect(() => {
    const fetchUserDetails = async () => {
      try {

        setLanguage(settings.language);
        setRowsPerPage(settings.rowsPerPage);
        setUpdateTime(settings.updateTime);

        const response = await axiosInstance.get("user");
        const user = response.data;
        setUserId(user.id);
        setUsername(user.userName);
        setEmail(user.email);
        initialUsername.current = user.userName;
        initialEmail.current = user.email;

        setPassword("");
        setConfirmPassword("");
      } catch (err: any) {
        setError(err.response?.data);
      }
    };

    fetchUserDetails();
  }, [settings]);

  const handleSave = async () => {
    try {
      settings.setLanguage(language);
      settings.setRowsPerPage(rowsPerPage);
      settings.setUpdateTime(updateTime);

      const dataToSend: any = {
        userId: userid,
        userName: username,
        email: email,
        role: 1
      };

      let hasChanges = false;

      if (username !== initialUsername.current) {
        dataToSend.userName = username;
        hasChanges = true;
      }
      if (email !== initialEmail.current) {
        dataToSend.email = email;
        hasChanges = true;
      }

      if (password != '') {
        hasChanges = true;
      }
      if (hasChanges) {
        if (password !== confirmPassword) {
          setSuccess(null);
          setError(t('passwordMismatch'));
          return;
        }
        await axiosInstance.put(
          "/admin/users",
          {
            dataToSend
          }
        );
      }
      setSuccess(t('updateSuccess'));
      setError(null);
    } catch (err: any) {
      setError(err.response?.data);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box sx={{ marginTop: 8, display: "flex", flexDirection: "column", alignItems: "center" }}>
        <Typography component="h1" variant="h5">
          {t('profileSettings')}
        </Typography>
        {error && <Alert severity="error">{error}</Alert>}
        {success && <Alert severity="success">{success}</Alert>}
        <Grid container spacing={2} sx={{ marginTop: 2 }}>
          <Grid item xs={12}>
            <TextField
              label={t('username')}
              fullWidth
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('email')}
              fullWidth
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('newPassword')}
              type="password"
              fullWidth
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('confirmPassword')}
              type="password"
              fullWidth
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
          </Grid>
          <Grid item xs={12}>
            <FormControl fullWidth>
              <InputLabel id="language-label">{t('language')}</InputLabel>
              <Select
                labelId="language-label"
                id="language-select"
                value={language}
                label={t('language')}
                onChange={(e) => setLanguage(e.target.value as "ru" | "en")}
              >
                <MenuItem value="ru">Русский</MenuItem>
                <MenuItem value="en">English</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('rowsPerPage')}
              type="number"
              fullWidth
              value={rowsPerPage}
              onChange={(e) => setRowsPerPage(parseInt(e.target.value, 10))}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('updateTime')}
              type="number"
              fullWidth
              value={updateTime}
              onChange={(e) => setUpdateTime(parseInt(e.target.value, 10))}
            />
          </Grid>
          <Grid item xs={12}>            <Button variant="contained" color="primary" onClick={handleSave}>
            {t('save')}
          </Button>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
};

export default UserProfile;
