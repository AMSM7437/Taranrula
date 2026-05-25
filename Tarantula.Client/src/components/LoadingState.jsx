import { Box, CircularProgress, Typography } from '@mui/material';

const LoadingState = () => {
    return (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                gap: 1.5,
                py: 1,
            }}
        >
            <CircularProgress
                size={18}
                thickness={4}
                sx={{ color: 'primary.main' }}
            />
            <Typography color="text.secondary" variant="body2">
                Searching indexed pages...
            </Typography>
        </Box>
    );
};

export default LoadingState;
