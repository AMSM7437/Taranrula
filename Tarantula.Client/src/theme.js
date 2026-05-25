import { createTheme } from '@mui/material/styles';

const colors = {
    background: '#202124',
    surface: '#303134',
    surfaceHover: '#303134',
    elevatedSurface: '#303134',
    border: '#3c4043',
    borderFocus: 'rgba(215,38,61,0.75)',
    primaryRed: '#D7263D',
    primaryHover: '#FF4057',
    textMain: '#e8eaed',
    textSecondary: '#9aa0a6',
    textMuted: '#71717a',
    urlText: '#bdc1c6',
    resultBlue: '#8ab4f8',
    resultBlueHover: '#aecbfa',
};

const theme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: colors.primaryRed,
        },
        background: {
            default: colors.background,
            paper: colors.surface,
        },
        text: {
            primary: colors.textMain,
            secondary: colors.textSecondary,
            disabled: colors.textMuted,
        },
    },
    shape: {
        borderRadius: 8,
    },
    typography: {
        fontFamily: ['Inter', 'Roboto', 'Arial', 'sans-serif'].join(','),
        h1: {
            fontWeight: 800,
            letterSpacing: 0,
        },
        h6: {
            fontWeight: 700,
            letterSpacing: 0,
        },
        button: {
            fontWeight: 700,
            textTransform: 'none',
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    boxShadow: 'none',
                    '&:hover': {
                        boxShadow: 'none',
                    },
                },
                containedPrimary: {
                    backgroundColor: colors.primaryRed,
                    '&:hover': {
                        backgroundColor: colors.primaryHover,
                    },
                },
            },
        },
        MuiCard: {
            styleOverrides: {
                root: {
                    backgroundImage: 'none',
                    backgroundColor: colors.elevatedSurface,
                    border: `1px solid ${colors.border}`,
                    borderRadius: 8,
                    boxShadow: '0 18px 44px rgba(0, 0, 0, 0.28)',
                },
            },
        },
        MuiChip: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    backgroundColor: 'rgba(215, 38, 61, 0.12)',
                    border: `1px solid ${colors.border}`,
                    color: colors.textSecondary,
                },
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    '& .MuiOutlinedInput-root': {
                        borderRadius: 999,
                        backgroundColor: colors.surface,
                        color: colors.textMain,
                        transition: 'border-color 160ms ease, background-color 160ms ease, box-shadow 160ms ease',
                        '& fieldset': {
                            borderColor: colors.border,
                        },
                        '&:hover': {
                            backgroundColor: colors.surfaceHover,
                        },
                        '&:hover fieldset': {
                            borderColor: colors.border,
                        },
                        '&.Mui-focused fieldset': {
                            borderColor: colors.borderFocus,
                        },
                        '&.Mui-focused': {
                            boxShadow: '0 0 0 4px rgba(215, 38, 61, 0.10)',
                        },
                    },
                    '& .MuiInputBase-input::placeholder': {
                        color: colors.textMuted,
                        opacity: 1,
                    },
                },
            },
        },
        MuiAlert: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    border: `1px solid ${colors.border}`,
                },
            },
        },
    },
    tarantula: colors,
});

export default theme;
