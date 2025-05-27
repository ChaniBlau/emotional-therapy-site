import { useDispatch, useSelector } from 'react-redux';
import { clearUser } from '../redux/userSlice';
import { useNavigate } from 'react-router-dom';
import Avatar from '@mui/material/Avatar';
import { useState } from 'react';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

const UserProfileMenu = () => {
  const user = useSelector(state => state.user.userInfo);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [show, setShow] = useState(false);

  if (!user) return null;

  const handleLogout = () => {
    dispatch(clearUser());
    setShow(false);
    navigate('/');
  };

return (
    <div className="dropdown" style={{ display: 'inline-block', marginLeft: '16px' }}>
        <button
            className="btn btn-link dropdown-toggle"
            type="button"
            id="profileDropdown"
            data-bs-toggle="dropdown"
            aria-expanded={show}
            onClick={() => setShow(!show)}
            style={{ padding: 0, border: 'none', boxShadow: 'none' }}
        >
            <Avatar sx={{ bgcolor: '#1976d2', width: 36, height: 36, fontSize: 20 }}>
                {user.name?.charAt(0).toUpperCase()}
            </Avatar>
        </button>
        <ul
            className={`dropdown-menu dropdown-menu-end${show ? ' show' : ''}`}
            aria-labelledby="profileDropdown"
            style={{ minWidth: '120px' }}
        >
            <li>
                <span className="dropdown-item-text" style={{ fontWeight: 'bold' }}>
                    {user.name}
                </span>
            </li>
            <li>
                <button className="dropdown-item" onClick={handleLogout}>
                    Logout
                </button>
            </li>
        </ul>
    </div>
);
};

export default UserProfileMenu;