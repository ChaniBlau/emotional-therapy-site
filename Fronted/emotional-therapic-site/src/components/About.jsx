import { Container, Typography, Box, Paper, Grid, Avatar, Button, Card, CardContent, Divider, Tooltip, Chip,} from "@mui/material";
import InfoIcon from "@mui/icons-material/Info";
import EmojiEmotionsIcon from "@mui/icons-material/EmojiEmotions";
import GroupIcon from "@mui/icons-material/Group";
import SupportAgentIcon from "@mui/icons-material/SupportAgent";
import EmailIcon from "@mui/icons-material/Email";

const features = [
  {
    icon: <EmojiEmotionsIcon color="primary" sx={{ fontSize: 40 }} />,
    title: "Child-Centered Approach",
    desc: "Personalized therapy plans focusing on each child's emotional needs and strengths.",
  },
  {
    icon: <GroupIcon color="secondary" sx={{ fontSize: 40 }} />,
    title: "Group Workshops",
    desc: "Interactive group sessions to foster social skills and peer support.",
  },
  {
    icon: <SupportAgentIcon color="success" sx={{ fontSize: 40 }} />,
    title: "Parental Guidance",
    desc: "Resources and sessions for parents to support their child's emotional journey.",
  },
];

const About = () => (
  <Box
    sx={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #e3f2fd 0%, #fce4ec 100%)",
      py: { xs: 4, md: 8 },
      px: 0,
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    }}
  >
    <Container maxWidth="xl" sx={{ px: { xs: 1, md: 4 } }}>
      <Paper
        elevation={8}
        sx={{
          p: { xs: 2, md: 6 },
          borderRadius: 5,
          background: "#fff",
          mb: 6,
          boxShadow: "0 8px 32px 0 rgba(31, 38, 135, 0.15)",
        }}
      >
        <Grid container spacing={3} alignItems="center">
          <Grid item xs={12} md={2} sx={{ textAlign: "center" }}>
            <Avatar
              sx={{
                bgcolor: "#1976d2",
                width: 72,
                height: 72,
                mx: "auto",
                boxShadow: 3,
              }}
            >
              <InfoIcon sx={{ fontSize: 48, color: "#fff" }} />
            </Avatar>
          </Grid>
          <Grid item xs={12} md={10}>
            <Typography
              variant="h3"
              component="h1"
              gutterBottom
              sx={{
                color: "#1976d2",
                fontWeight: 700,
                letterSpacing: 1,
              }}
            >
              About Emotional Therapy for Children
            </Typography>
            <Typography variant="subtitle1" sx={{ color: "#555" }}>
              We empower children and families through compassionate, evidence-based emotional support. Our mission is to provide a safe, nurturing environment where every child can thrive emotionally and socially.
            </Typography>
          </Grid>
        </Grid>
        <Divider sx={{ my: 4 }} />
        <Grid
          container
          spacing={4}
          alignItems="stretch"
          justifyContent="center"
        >
          {features.map((feature, idx) => (
            <Grid item xs={12} sm={6} md={4} key={idx} sx={{ display: "flex" }}>
              <Card
                elevation={0}
                sx={{
                  borderRadius: 3,
                  background: "#f5fafd",
                  width: "100%",
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                  height: "100%",
                  boxShadow: 2,
                }}
              >
                <CardContent sx={{ textAlign: "center", flexGrow: 1 }}>
                  {feature.icon}
                  <Typography variant="h6" sx={{ mt: 2, fontWeight: 600 }}>
                    {feature.title}
                  </Typography>
                  <Typography variant="body2" sx={{ mt: 1, color: "#666" }}>
                    {feature.desc}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
        <Divider sx={{ my: 4 }} />
        <Box sx={{ textAlign: "center" }}>
          <Tooltip title="Send us a message" arrow>
            <Button
              variant="contained"
              size="large"
              startIcon={<EmailIcon />}
              sx={{
                background: "linear-gradient(90deg, #42a5f5 0%, #ab47bc 100%)",
                color: "#fff",
                px: 5,
                py: 1.5,
                fontWeight: 600,
                fontSize: "1.1rem",
                borderRadius: 3,
                boxShadow: 2,
                "&:hover": {
                  background: "linear-gradient(90deg, #1976d2 0%, #8e24aa 100%)",
                },
              }}
              onClick={() => {
                const message = window.prompt("How can we help you? Please enter your message:");
                if (message) {
                  alert("Thank you for reaching out!\n\nYour message: " + message);
                }
              }}
            >
              Contact Us
            </Button>
          </Tooltip>
          <Box sx={{ mt: 3 }}>
            <Chip
              label="Confidential & Secure"
              color="success"
              icon={<SupportAgentIcon />}
              sx={{ fontWeight: 500, fontSize: "1rem" }}
            />
          </Box>
        </Box>
      </Paper>
    </Container>
  </Box>
);

export default About;
