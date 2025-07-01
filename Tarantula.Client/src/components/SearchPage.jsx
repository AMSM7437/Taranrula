import React, { useState } from 'react';
import SearchBox from './SearchBox';
import ResultsList from './ResultsList';

const SearchPage = () => {
    const [query, setQuery] = useState('');

    return (
        <div className="max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold text-center mb-6">🔍 Tarantula Search</h1>
            <SearchBox onSearch={setQuery} />
            <ResultsList query={query} />
        </div>
    );
};

export default SearchPage;
