function initializeRequest(sender, args) {
  var senderId = sender._activeElement.id;
  if (senderId) {
    var op = senderId.match(/(?:[A-Z0-9]+_)*([A-Z]+)/i)[1];
    var msg = senderId.match(/(?:[A-Z0-9]+_)*(?:[A-Z]+)([0-9]*)/i)[1];
    if (op == "Delete" && msg) {
      if (!confirm("OK to delete message number " + msg + "?"))
        args.set_cancel(true);
    }
  }
}
