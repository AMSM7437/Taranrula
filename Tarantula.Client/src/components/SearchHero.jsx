import CloseIcon from '@mui/icons-material/Close';
import SearchIcon from '@mui/icons-material/Search';
import { Box, IconButton, InputAdornment, Stack, TextField, Typography } from '@mui/material';
import tarantulaLogo from '../../../src/assets/Tarantulalogo.png';

const SearchHero = ({ query, loading, onQueryChange, onSearch }) => {
    return (
        <Box component="header" sx={{ textAlign: 'center' }}>
            <Stack spacing={1.5} alignItems="center">
                <Box>
                    <Box
                        component="img"
                        src={tarantulaLogo}
                        alt="Tarantula search engine logo"
                        sx={{
                            width: { xs: 260, sm: 340, md: 420 },
                            height: 'auto',
                            objectFit: 'contain',
                            display: 'block',
                            mx: 'auto',
                        }}
                    />
                    <Typography
                        variant="subtitle1"
                        color="text.secondary"
                        sx={{ mt: 0.75, fontSize: { xs: '0.95rem', md: '1rem' } }}
                    >
                        to live is to crawl
                    </Typography>
                </Box>

                <Box
                    component="form"
                    onSubmit={onSearch}
                    sx={{
                        width: '100%',
                        mt: 2.5,
                    }}
                >
                    <TextField
                        fullWidth
                        value={query}
                        onChange={(event) => onQueryChange(event.target.value)}
                        placeholder="Search indexed pages..."
                        autoFocus
                        slotProps={{
                            input: {
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <SearchIcon sx={{ color: 'text.disabled' }} />
                                    </InputAdornment>
                                ),
                                endAdornment: query && (
                                    <InputAdornment position="end">
                                        <IconButton
                                            aria-label="Clear search"
                                            edge="end"
                                            size="small"
                                            disabled={loading}
                                            onClick={() => onQueryChange('')}
                                            sx={{
                                                color: 'text.disabled',
                                                '&:hover': {
                                                    color: 'text.secondary',
                                                    bgcolor: 'transparent',
                                                },
                                            }}
                                        >
                                            <CloseIcon fontSize="small" />
                                        </IconButton>
                                    </InputAdornment>
                                ),
                                sx: {
                                    minHeight: 56,
                                },
                            },
                        }}
                    />
                </Box>

                <Typography variant="body2" color="text.disabled" sx={{ minHeight: 20 }}>
                    Crawler-backed local search
                </Typography>
            </Stack>
        </Box>
    );
};

export default SearchHero;
