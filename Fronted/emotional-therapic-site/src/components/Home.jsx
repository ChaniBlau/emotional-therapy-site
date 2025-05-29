import React from 'react';
import { Box, Typography, Button, Container, Paper } from '@mui/material';
import { useNavigate } from 'react-router-dom'; // *** חובה לייבא את useNavigate ***

import smalllogo from '../assets/smallsmalllogo.png';

const Home = () => {
  const navigate = useNavigate();

  const handleLearnMoreClick = () => {
    navigate('/about');
  };

  const handleSignUpClick = () => {
    navigate('/signup');
  };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #e3f2fd 0%, #fce4ec 100%)',
        py: 4,
      }}
    >
      <Container maxWidth="md">
        <Paper
          elevation={8}
          sx={{
            p: { xs: 4, md: 6 },
            borderRadius: 4,
            textAlign: 'center',
            background: '#fff',
            boxShadow: '0 12px 24px rgba(0, 0, 0, 0.15)',
          }}
        >
          <Box
            component="img"
            src={smalllogo}
            alt="Institute Logo"
           sx={{
              width: { xs: 180, sm: 220, md: 280 }, // רוחב רחב יותר
              height: { xs: 80, sm: 100, md: 120 }, // גובה קצר יותר
              borderRadius: '50%', 
              mb: 3,
              boxShadow: '0 4px 10px rgba(0, 0, 0, 0.1)',
            objectFit: 'contain', // חשוב: מבטיח שהתמונה תותאם לגודל בלי להתעוות
            }}
          />

          <Typography
            variant="h3"
            component="h1"
            gutterBottom
            sx={{
              color: '#3f51b5',
              fontWeight: 700,
              mb: 2,
            }}
          >
            Welcome to the Institute for Emotional Therapy for Children
          </Typography>

          <Typography
            variant="h6"
            color="text.secondary"
            sx={{
              mb: 4,
              lineHeight: 1.6,
              maxWidth: '600px',
              mx: 'auto',
            }}
          >
            A safe and nurturing space dedicated to fostering emotional well-being and growth in children and their families.
          </Typography>

          <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center', gap: 2 }}>
            <Button
              variant="contained"
              color="primary"
              size="large"
              onClick={handleLearnMoreClick}
              sx={{
                px: 4,
                py: 1.5,
                fontWeight: 600,
                borderRadius: 2,
                boxShadow: '0 4px 10px rgba(25, 118, 210, 0.3)',
                '&:hover': {
                  boxShadow: '0 6px 14px rgba(25, 118, 210, 0.4)',
                },
              }}
            >
              Learn More About Us
            </Button>
            <Button
              variant="outlined"
              color="secondary"
              size="large"
              onClick={handleSignUpClick}
              sx={{
                px: 4,
                py: 1.5,
                fontWeight: 600,
                borderRadius: 2,
                borderColor: '#ab47bc',
                color: '#ab47bc',
                '&:hover': {
                  backgroundColor: 'rgba(171, 71, 188, 0.04)',
                  borderColor: '#9c27b0',
                  color: '#9c27b0',
                },
              }}
            >
              Sign Up Now
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default Home;