import React, { useState } from 'react';

const SearchBox = ({ onSearch }) => {
    const [input, setInput] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        onSearch(input);
    };

    return (
        <form onSubmit={handleSubmit} className="flex justify-center mb-4">
            <input
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
                placeholder="Search..."
                className="w-full md:w-2/3 p-2 rounded-l border border-gray-300"
            />
            <button
                type="submit"
                className="bg-blue-600 text-white px-4 py-2 rounded-r hover:bg-blue-700"
            >
                Search
            </button>
        </form>
    );
};

export default SearchBox;
