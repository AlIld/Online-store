import React, {Component} from 'react';
import authService from "./api-authorization/AuthorizeService";
import {OrderDetails} from "./OrderDetails";
import {Order} from "./Order";

export class OrderDetailsPage extends Component {
  static displayName = OrderDetailsPage.name;

  constructor(props) {
    super(props);
    this.state = {
      order: '',
      loading: true
    };
  }

  componentDidMount() {
    this.populateOrder();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <OrderDetails order={this.state.order} history={this.props.history}/>;

    return (
      <div>
        {contents}
      </div>
    );
  }

  async populateOrder() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/order/getOrder?orderId=' + this.props.match.params.orderId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`}
    });
    const data = (await response.json());
    this.setState({
      order: data,
      loading: false,
    });
  }
}
