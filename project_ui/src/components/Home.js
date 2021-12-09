// import { BrowserRouter as Route } from 'react-router-dom';
import React from 'react';
import { Route,  BrowserRouter as Router, Switch } from 'react-router-dom'
import Index from './pages/Index';
import Login from './pages/Login';
import Registration from './pages/Registration';
import NavBar from './NavBar';
import { withRouter } from "react-router";
import Logout from './pages/Logout';
import Film from './pages/Film';
import { simpleService } from '../services/simpleService';
import './css/index.css'
import Actor from './pages/Actor';
import Directors from './pages/Directors';
import Producer from './pages/Producer';
import Popular from './pages/Popular';
class Home extends React.Component {
  
  constructor(props)
  {
    super(props)
    this.state = {
     
      }
     
    
  }
  render(){
    return ( <>
      <Router>
          <Switch>
            <Route exact path='/' component={() =><Index ></Index>} ></Route>
            <Route path='/login' component={Login}></Route> 
            <Route path='/logout' component={Logout}></Route> 
            <Route path='/registration' component={Registration}></Route>
            <Route path='/actors/:id' component={Actor}></Route>
            <Route path='/directors/:id' component={Directors}></Route>
             <Route path='/producers/:id' component={Producer}></Route>
             <Route path='/Populars' component={Popular}></Route>
            {/* <Route path='/selections/:id' component={Registration}></Route> */}
            <Route path="/films/:id" component={Film}/> 
            </Switch>
      </Router></>
    );
  }
    

}
export default Home;