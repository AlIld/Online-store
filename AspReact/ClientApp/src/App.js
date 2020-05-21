import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'
import {Admin} from "./components/Admin";
import {CategoryProducts} from "./components/CategoryProducts";
import {Cart} from "./components/Cart";
import {Orders} from "./components/Orders";
import {OrderPay} from "./components/OrderPay";
import {OrderDetailsPage} from "./components/OrderDetailsPage";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        {/*<Route path='/counter' component={Counter} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />*/}
        <AuthorizeRoute exact path='/Admin' component={Admin} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        <Route exact path='/categoryProducts/:categoryId/' component={CategoryProducts}/>
        <AuthorizeRoute exact path='/cart' component={Cart}/>
        
        <AuthorizeRoute exact path='/orders' component={Orders}/>
        <AuthorizeRoute exact path='/orders/pay/:orderId/' component={OrderPay}/>
        <AuthorizeRoute exact path='/orders/details/:orderId/' component={OrderDetailsPage}/>
      </Layout>
    );
  }
}
