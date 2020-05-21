import React, {Component} from 'react';
import authService from "./api-authorization/AuthorizeService";
import {Order} from "./Order";
import {Table} from "reactstrap";

export class Orders extends Component {
  static displayName = Orders.name;

  constructor(props) {
    super(props);
    this.state = {
      orders: [],
      loading: true
    };
  }

  componentDidMount() {
    this.populateOrders();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      :
      this.state.orders.map(x => <Order order={x} history={this.props.history}/>);

    return (
      <div>
        <h1>List of your orders:</h1>
        <Table>
          <thead>
          <tr>
            <th>#</th>
            <th>Date and time</th>
            <th>Price</th>
            <th>Details</th>
            <th>Delivered</th>
            <th>Paid</th>
          </tr>
          </thead>
          <tbody>
          {contents}
          </tbody>
        </Table>
      </div>
    );
  }

  async populateOrders() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/order/getOrders', {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`}
    });
    const data = (await response.json());
    this.setState({
      orders: data,
      loading: false,
    });
  }
}
