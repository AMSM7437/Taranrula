import React, { useState } from 'react';
import { Box, Container, CssBaseline, Stack, ThemeProvider, Typography } from '@mui/material';
import LoadingState from './components/LoadingState';
import ResultCard from './components/ResultCard';
import SearchHero from './components/SearchHero';
import theme from './theme';

const App = () => {
    const [query, setQuery] = useState('');
    const [searchedQuery, setSearchedQuery] = useState('');
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async (event) => {
        event.preventDefault();

        const trimmedQuery = query.trim();
        if (!trimmedQuery) return;

        setLoading(true);
        setError('');
        setSearchedQuery(trimmedQuery);

        try {
            const response = await fetch(`/Tarantula/search?query=${encodeURIComponent(trimmedQuery)}`);

            if (!response.ok) {
                throw new Error(`Search request failed with status ${response.status}`);
            }

            const data = await response.json();
            setResults(data);
        } catch (error) {
            console.error('Search failed:', error);
            setResults([]);
            setError('Could not connect to the API. Make sure Tarantula.API is running on port 5073.');
        } finally {
            setLoading(false);
        }
    };

    const hasSearched = searchedQuery.length > 0;
    const hasResults = results.length > 0;

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
                <Container
                    maxWidth={false}
                    sx={{
                        width: '100%',
                        maxWidth: 720,
                        minHeight: '100vh',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: hasSearched || loading || error ? 'flex-start' : 'center',
                        pt: hasSearched || loading || error ? { xs: 5, md: 8 } : 0,
                        pb: { xs: 4, md: 7 },
                        transform: hasSearched || loading || error ? 'none' : 'translateY(-6vh)',
                        transition: 'transform 180ms ease, padding-top 180ms ease',
                    }}
                >
                    <SearchHero
                        query={query}
                        loading={loading}
                        onQueryChange={setQuery}
                        onSearch={handleSearch}
                    />

                    <Box
                        component="section"
                        aria-live="polite"
                        sx={{
                            mt: hasSearched || loading || error ? 3 : 0,
                            pt: hasSearched || loading || error ? 2.5 : 0,
                            borderTop: hasSearched || loading || error ? '1px solid #3c4043' : 'none',
                        }}
                    >
                        {loading && <LoadingState />}

                        {!loading && error && (
                            <Typography color="primary.main" textAlign="center" variant="body2">
                                {error}
                            </Typography>
                        )}

                        {!loading && !error && hasSearched && !hasResults && (
                            <Typography color="text.disabled" textAlign="center" variant="body2">
                                No results found.
                            </Typography>
                        )}

                        {!loading && !error && hasResults && (
                            <Stack spacing={0}>
                                <Typography variant="body2" color="text.secondary" sx={{ mb: 1.5 }}>
                                    Results for "{searchedQuery}"
                                </Typography>

                                {results.map((result, index) => (
                                    <ResultCard
                                        key={`${result.url}-${index}`}
                                        result={result}
                                    />
                                ))}
                            </Stack>
                        )}
                    </Box>
                </Container>
            </Box>
        </ThemeProvider>
    );
};

export default App;
