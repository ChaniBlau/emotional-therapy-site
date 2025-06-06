import { BrowserRouter, NavLink, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './components/Home'
import LogIn from './components/LogIn'
import SignUp from './components/SignUp'
import About from './components/About'
import ClientDashboard from './components/ClientDashboard'
import TherapistDashboard from './components/TherapistDashboard'
import UserProfileMenu from './components/UserProfileMenu'; 

function App() {
  return (
    <>
          <BrowserRouter>
        <div id='navDiv'>
          <nav>
              <NavLink to="/"  activeclassname="active">Home</NavLink>
              <NavLink to="/about"  activeclassname="active">About</NavLink> 
              <NavLink to="/login"  activeclassname="active">Log In</NavLink>
              <NavLink to="/signup"  activeclassname="active">Sign Up</NavLink>
              <UserProfileMenu />

          </nav>
        </div>
        <Routes>
          <Route path='/' element={<Home />} />
          <Route path='/about' element={<About/>} />
          <Route path='/login' element={<LogIn />} />
          <Route path='/signup' element={<SignUp />} />
          <Route path="/client-dashboard" element={<ClientDashboard />} />
          <Route path="/therapist-dashboard" element={<TherapistDashboard />} />  
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App;
