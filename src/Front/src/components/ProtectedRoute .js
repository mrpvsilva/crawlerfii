import { Navigate, useOutlet } from "react-router-dom";
import { Container } from '@mui/material'

import { useAuth } from "../hooks/useAuth";
import Navigation from "../components/Navigation"

export const ProtectedRoute = () => {
    const outlet = useOutlet();
    const { user } = useAuth() || {};

    if (!user) {
        // user is not authenticated
        return <Navigate to="/login" />;
    }

    return (
        <>
            <Navigation />
            <Container>
                {outlet}
            </Container>
        </>
    );
}