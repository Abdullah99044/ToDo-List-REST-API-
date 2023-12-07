import { useContext } from 'react'
import './App.css'
import { Routes ,  Route , Link} from "react-router-dom";
import Login from './Pages/Login';
import Register from './Pages/Register';
import Home from './Pages/Home';
import PrivateWrapper from './Pages/Auth/ProtectedRoute';
import {  AuthContext  } from './Pages/Auth/AuthenticationProvider.tsx'

 
function App() {
  

  const  logIn  = useContext(AuthContext);

  return (


    <>


    <nav className="navbar navbar-expand-lg    bg-black ">

      <div className="container-fluid"  >

          <a className="navbar-brand"  > <h1 className='text-light'> Todo list </h1></a>

          <div className="collapse navbar-collapse" id="navbarSupportedContent">

            <ul className="navbar-nav me-auto mb-2 mb-lg-0">

              

              {logIn?.authenticated ? (

                <>

                  <li className="nav-item">
                    <Link className="nav-link active text-light" aria-current="page" to='/Home'  >  Home </Link> 
                  </li>
                                  
                  <li className="nav-item">
                    <button className="nav-link active text-light" onClick={logIn?.logout}  >  Logout  </button> 
                  </li>

                </>

              ) : ( 


                <>
              
                  <li className="nav-item">
                    <Link className="nav-link active text-light" aria-current="page" to='/login'  >  Login </Link> 
                  </li>

                  <li className="nav-item">
                    <Link className="nav-link active text-light" aria-current="page" to='/Register'  >  Register </Link> 
                  </li>
                
                </>
                
              )}
              

            </ul>

          </div>

      </div>

    </nav>


   
   
     <Routes>
        <Route path='/login' element={ <Login />} />  
        <Route path='/Register' element={ <Register />} />


        <Route path='/Home' element={ <PrivateWrapper> <Home /> </PrivateWrapper>} />


      </Routes>
  
    

  
 

     
 

      
    </>
  )
}

export default App
