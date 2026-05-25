import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');
    const apiUrl = env.VITE_API_URL || 'https://localhost:7226';

    return {
        plugins: [plugin()],
        server: {
            host: '0.0.0.0',
            port: 50964,
            proxy: {
                '/Tarantula': {
                    target: apiUrl,
                    changeOrigin: true,
                    secure: false,
                },
            },
        },
    };
})
