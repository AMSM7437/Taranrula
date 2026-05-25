import { Card, CardContent, Typography } from '@mui/material';

const EmptyState = ({
    title = 'Ready to search',
    message = 'Enter a query to search indexed pages.',
}) => {
    return (
        <Card>
            <CardContent sx={{ py: 6, px: 3, textAlign: 'center' }}>
                <Typography variant="h6" sx={{ mb: 1 }}>
                    {title}
                </Typography>
                <Typography color="text.secondary">
                    {message}
                </Typography>
            </CardContent>
        </Card>
    );
};

export default EmptyState;
