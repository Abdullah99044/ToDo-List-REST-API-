import {   } from 'react'
import './App.css'
import { BrowserRouter as Router    , Route , Link} from "react-router-dom";
import Login from './Pages/Login';
import Register from './Pages/Register';
 
 
function App() {
 
  return (


    <>


    <nav className="navbar navbar-expand-lg    bg-black ">

      <div className="container-fluid"  >

          <a className="navbar-brand"  > <h1 className='text-light'> Todo list </h1></a>

          <div className="collapse navbar-collapse" id="navbarSupportedContent">

            <ul className="navbar-nav me-auto mb-2 mb-lg-0">

              <li className="nav-item">
                <Link className="nav-link active text-light" aria-current="page" to='/login'  >  Login </Link> 
              </li>

              <li className="nav-item">
                <Link className="nav-link active text-light" aria-current="page" to='/Register'  >  Register </Link> 
              </li>

            </ul>

          </div>

      </div>

    </nav>


    <Router>
   
     
      <Route path='/login' element={ <Login />} />  
      <Route path='/Register' element={ <Register />} />
       
  
    

    </Router>
 

     
 

      
    </>
  )
}

export default App
