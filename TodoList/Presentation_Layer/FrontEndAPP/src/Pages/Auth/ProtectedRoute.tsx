import { Navigate } from "react-router-dom";
import { AuthContext } from  "./AuthenticationProvider.tsx"
import { useContext , ReactNode } from "react";


interface Props  {

    children : ReactNode
}

const PrivateWrapper = ({ children }: Props  ) => {


    const auth = useContext(AuthContext)

    return auth?.authenticated ? children : <Navigate to="/login" replace />;

};


export default PrivateWrapper ;