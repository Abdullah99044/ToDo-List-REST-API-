import {   } from 'react'
import './App.css'
import { Routes , Route , Link} from "react-router-dom";
import Login from './Pages/Login';
 
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

            </ul>

          </div>

      </div>

    </nav>
    <Routes>

      <Route path='/login' element={ <Login />} />  

    </Routes>

      
    </>
  )
}

export default App
