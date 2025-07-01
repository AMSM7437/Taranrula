import React, { useState } from 'react';

const App = () => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!query.trim()) return;

        setLoading(true);
        try {
            const response = await fetch(`http://localhost:5073/Tarantula/search?query=${encodeURIComponent(query)}`);
            const data = await response.json();
            setResults(data);
        } catch (error) {
            console.error('Search failed:', error);
            setResults([]);
        }
        setLoading(false);
    };

    return (
        <div style={styles.container}>
            <header style={styles.header}>
                <h1 style={styles.logo}>Tarantula<span style={styles.tagline}>to live is to crawl</span></h1>
            </header>

            <main style={styles.main}>
                <form onSubmit={handleSearch} style={styles.searchForm}>
                    <div style={styles.searchContainer}>
                        <input
                            type="text"
                            placeholder="Search something..."
                            value={query}
                            onChange={(e) => setQuery(e.target.value)}
                            style={styles.searchInput}
                            autoFocus
                        />
                        {query && (
                            <button
                                type="button"
                                style={styles.clearButton}
                                onClick={() => { setQuery(''); setResults([])  }}
                            >
                                ✕
                            </button>
                        )}
                    </div>
                  
                </form>

                <div style={styles.resultsContainer}>
                    {loading ? (
                        <div style={styles.loading}>
                            <span style={styles.spinner}></span>
                            <p>Searching...</p>
                        </div>
                    ) : results.length === 0 ? (
                        <div style={styles.initialState}>
                            {/*<svg style={styles.initialIcon} viewBox="0 0 24 24">*/}
                            {/*    <path d="M15.5 14h-.79l-.28-.27a6.5 6.5 0 0 0 1.48-5.34c-.47-2.78-2.79-5-5.59-5.34a6.505 6.505 0 0 0-7.27 7.27c.34 2.8 2.56 5.12 5.34 5.59a6.5 6.5 0 0 0 5.34-1.48l.27.28v.79l4.25 4.25c.41.41 1.08.41 1.49 0 .41-.41.41-1.08 0-1.49L15.5 14zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z" />*/}
                            {/*</svg>*/}
                            {/*<p style={styles.initialText}>Enter a search query to begin</p>*/}
                        </div>
                    ) : (
                        <div style={styles.resultsList}>
                            {results.map((res, i) => (
                                <div key={i} style={styles.resultCard}>
                                    <div style={styles.resultContent}>
                                        <a href={res.url} target="_blank" rel="noopener noreferrer" style={styles.resultUrl}>
                                            {new URL(res.url).hostname}
                                        </a>
                                        <a href={res.url} target="_blank" rel="noopener noreferrer" style={styles.resultTitle}>
                                            {res.title || 'Untitled'}
                                        </a>
                                        {res.meta && <p style={styles.resultMeta}>{res.meta}</p>}
                                    </div>
                                    {/\.(jpg|jpeg|png|gif|webp)$/i.test(res.url) && (
                                        <div style={styles.resultImage}>
                                            <img src={res.url} alt="" style={styles.image} />
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </main>

            <footer style={styles.footer}>
                <p style={styles.footerText}>© {new Date().getFullYear()} Tarantula by AMSM7437</p>
            </footer>
        </div>
    );
};

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh',
        minWidth:'100dvw',
        backgroundColor: '#000',
        fontFamily: "'Roboto', 'Arial', sans-serif",
        color: '#202124',
    },
    header: {
        padding: '20px 0',
        textAlign: 'center',
    },
    logo: {
        fontSize: '3rem',
        fontWeight: '500',
        color: '#A70B20',
        margin: 0,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: '10px',
    },
    tagline: {
        fontSize: '1rem',
        color: '#5f6368',
        fontWeight: 'normal',
    },
    main: {
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        padding: '20px',
        width: '100%',
        maxWidth: '800px',
        margin: '0 auto',
    },
    searchForm: {
        width: '100%',
        maxWidth: '600px',
        marginBottom: '30px',
    },
    searchContainer: {
        position: 'relative',
        width: '100%',
        marginBottom: '20px',
    },
    searchInput: {
        width: '100%',
        padding: '15px 15px ',
        fontSize: '1rem',
        borderRadius: '24px',
        border: '1px solid #dfe1e5',
        outline: 'none',
        boxShadow: '0 2px 5px rgba(0,0,0,0.1)',
        transition: 'box-shadow 0.3s',
    },
    searchInputFocus: {
        boxShadow: '0 2px 8px rgba(0,0,0,0.2)',
    },
    clearButton: {
        position: 'absolute',
        right: '0px',
        top: '50%',
        transform: 'translateY(-50%)',
        background: 'none',
        border: 'none',
        cursor: 'pointer',
        fontSize: '1rem',
        color: '#70757a',
        padding: '5px',
    },
    buttonContainer: {
        display: 'flex',
        justifyContent: 'center',
        gap: '10px',
    },
    searchButton: {
        padding: '10px 20px',
        fontSize: '0.9rem',
        backgroundColor: '#f8f9fa',
        border: '1px solid #f8f9fa',
        borderRadius: '4px',
        color: '#3c4043',
        cursor: 'pointer',
        transition: 'border-color 0.3s, box-shadow 0.3s',
        display: 'flex',
        alignItems: 'center',
        gap: '8px',
    },
    spinner: {
        display: 'inline-block',
        width: '18px',
        height: '18px',
        border: '2px solid rgba(0,0,0,0.1)',
        borderLeftColor: '#4285F4',
        borderRadius: '50%',
        animation: 'spin 1s linear infinite',
    },
    resultsContainer: {
        width: '100%',
        flex: 1,
    },
    loading: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        gap: '20px',
        height: '200px',
    },
    initialState: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        gap: '20px',
        height: '300px',
        color: '#5f6368',
    },
    initialIcon: {
        width: '80px',
        height: '80px',
        fill: '#e0e0e0',
    },
    initialText: {
        fontSize: '1.2rem',
        margin: 0,
    },
    resultsList: {
        display: 'flex',
        flexDirection: 'column',
        gap: '20px',
    },
    resultCard: {
        backgroundColor: '#f0f0f0',
        borderRadius: '8px',
        padding: '20px',
        boxShadow: '0 1px 6px rgba(32,33,36,0.08)',
        display: 'flex',
        gap: '20px',
    },
    resultContent: {
        flex: 1,
    },
    resultUrl: {
        color: '#000',
        fontSize: '0.8rem',
        marginBottom: '5px',
        display: 'block',
        textDecoration: 'none',
    },
    resultTitle: {
        color: '#1a0dab',
        fontSize: '1.2rem',
        marginBottom: '8px',
        display: 'block',
        textDecoration: 'none',
        fontWeight: '500',
    },
    resultTitleHover: {
        textDecoration: 'underline',
    },
    resultMeta: {
        color: '#4d5156',
        fontSize: '0.9rem',
        margin: 0,
        lineHeight: 1.4,
    },
    resultImage: {
        width: '120px',
        height: '120px',
        borderRadius: '4px',
        overflow: 'hidden',
        flexShrink: 0,
    },
    image: {
        width: '100%',
        height: '100%',
        objectFit: 'cover',
    },
    footer: {
        padding: '20px',
        textAlign: 'center',
        backgroundColor: '#0f0f0f',
        color: '#70757a',
        fontSize: '0.9rem',
    },
    footerText: {
        margin: 0,
    },
};



export default App;