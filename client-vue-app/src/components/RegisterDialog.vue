<template>
  <v-dialog
      v-model="dialog"
      class="auth-dialog"
  >
    <template v-slot:activator="{ }">
      <v-btn
          v-bind="$attrs"
          color="primary"
          plain
          dark
          @click.stop="dialog = true"
      >
        <v-icon left icon="mdi-account-plus-outline"></v-icon>
        Create Account
      </v-btn>
    </template>
    <v-card>
      <v-card-title>
        <span class="text-h5">Register</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                  autocomplete="new-password"
                  label="Username*"
                  required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                  autocomplete="new-password"
                  label="Email*"
                  required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                  autocomplete="new-password"
                  label="Password*"
                  type="password"
                  required
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
            flat
            @click="dialog = false"
        >
          Close
        </v-btn>
        <v-btn
            flat
            @click="diadialoglog = false"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: 'RegisterDialog',
    data: () => ({
      dialog: false,
    }),
  }
</script>

<style scoped>

</style>
