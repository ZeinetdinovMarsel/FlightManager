import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material';
import { useTranslation } from 'react-i18next';
import loadingGif from '../assets/loader.gif';
interface ImagePopupProps {
    imageUrl: string | null;
    onClose: () => void;
}

const ImagePopup: React.FC<ImagePopupProps> = ({ imageUrl, onClose }) => {
    const { t } = useTranslation();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (imageUrl) {
            setLoading(true);
            const img = new Image();
            img.src = imageUrl;
            img.onload = () => setLoading(false);
            img.onerror = () => setLoading(false);
        }
    }, [imageUrl]);

    return (
        <Dialog open={!!imageUrl} onClose={onClose}>
            <DialogTitle>{t("image")}</DialogTitle>
            <DialogContent>
                {loading ? (
                    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: '20px' }}>
                        <img src={loadingGif} alt={t("loading")} style={{ width: '5%', height: 'auto' }} />
                    </div>
                ) : (
                    imageUrl ? (
                        <img src={imageUrl} alt={t("airplane")} style={{ width: '100%', height: 'auto' }} />
                    ) : (
                        <p>{t("imageLoadError")}</p>
                    )
                )}
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="primary">
                    {t("close")}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ImagePopup;