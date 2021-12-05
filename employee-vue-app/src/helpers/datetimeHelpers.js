import moment from 'moment'

export function fromNow(datetime) {
  return moment(datetime).fromNow()
}
