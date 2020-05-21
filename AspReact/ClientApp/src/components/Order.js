import React, {Component} from 'react';
import {Button} from "reactstrap";

export class Order extends Component {
  static displayName = Order.name;

  constructor(props) {
    super(props);
  }
    
  render() {
    console.log(this.props.order);
    return (
      <tr>
        <th scope="row">{this.props.order.id}</th>
        <td>{new Date(this.props.order.dateTime).toLocaleString()}</td>
        <td>{this.props.order.fullPrice}</td>
        <td>
          <Button onClick={() => this.props.history.push('/orders/details/' + this.props.order.id)}>Details</Button>
        </td>
        <td>{this.props.order.isDelivered ? 'Delivered' : 'Not delivered'}</td>
        <td>
          {
            this.props.order.isPaid
              ? 'Paid'
              : <Button onClick={() => this.props.history.push('/orders/pay/' + this.props.order.id)}>Pay</Button>
          }
        </td>
      </tr>
    );
  }
}