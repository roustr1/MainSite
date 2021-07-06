import MsModal from "../ms-modal"

export default {
  name: 'ms-simple-modal',
  props: {
    lorem: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      header: 'Ошибка1',
      body: 'Ошибка',
    }
  },
  methods: {
    close() {
      this.$modals.close(true)
    },
    cancel() {
      this.$modals.dismiss()
    }
  },
  render() {
    return (
      <MsModal>
        <div slot="footer" style="display:flex;justify-content: space-between;">
          <button style="padding: 0px 30px;" class="btn btn-defaultMainSite" onClick:prevent={this.cancel}>Нет</button>
          <span style="padding: 0px 30px;" class="btn btn-defaultMainSite" onClick:prevent={this.close}>Да</span>
        </div>
      </MsModal>
    )
  },
}