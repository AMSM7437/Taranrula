import { Box, Stack, Typography } from '@mui/material';

const ResultCard = ({ result }) => {
    const domain = getDomain(result.url);

    return (
        <Box
            component="article"
            sx={{
                py: 2.5,
                borderBottom: '1px solid #3c4043',
            }}
        >
            <Stack spacing={0.75}>
                <Typography
                    variant="body2"
                    sx={{ color: '#bdc1c6', overflowWrap: 'anywhere' }}
                >
                    {domain || result.url}
                </Typography>

                <Typography
                    variant="h6"
                    component="a"
                    href={result.url}
                    target="_blank"
                    rel="noopener noreferrer"
                    sx={{
                        color: '#8ab4f8',
                        textDecoration: 'none',
                        fontSize: '1.12rem',
                        lineHeight: 1.35,
                        '&:hover': {
                            color: '#aecbfa',
                            textDecoration: 'underline',
                        },
                    }}
                >
                    {result.title || 'Untitled page'}
                </Typography>

                <Typography sx={{ color: '#e8eaed', lineHeight: 1.65 }}>
                    {result.meta || 'No description was stored for this page.'}
                </Typography>

                <Typography
                    component="a"
                    href={result.url}
                    target="_blank"
                    rel="noopener noreferrer"
                    variant="body2"
                    sx={{
                        color: '#bdc1c6',
                        overflowWrap: 'anywhere',
                        textDecoration: 'none',
                        '&:hover': {
                            color: '#e8eaed',
                            textDecoration: 'underline',
                        },
                    }}
                >
                    {result.url}
                </Typography>
            </Stack>
        </Box>
    );
};

const getDomain = (url) => {
    try {
        return new URL(url).hostname;
    } catch {
        return '';
    }
};

export default ResultCard;
